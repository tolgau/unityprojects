using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TowerBaseScript : MonoBehaviour {
	protected List<GameObject> tilesInRange = new List<GameObject>();
	protected List<GameObject> combinedList = new List<GameObject>();
	protected LevelScript levelScript;
	protected GameObject target;
	protected Material defaultMaterial;
	public Material blinkMaterial;
	protected float fireRate;
	protected float damage;
	protected float lastFireStarted;
	
	// Use this for initialization
	protected virtual void Start () {
		defaultMaterial = this.renderer.material;
		target = null;
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		PopulateRangeWithTiles();
		AddListsTogether();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		MonitorRange();
	}
	
	protected virtual void PopulateRangeWithTiles(){
		
	}
	
	protected virtual void AddListsTogether(){
		combinedList.Clear ();
		foreach(GameObject tile in tilesInRange){
			TileScript tileScript = tile.GetComponent<TileScript>();
			combinedList.AddRange(tileScript.tileOccupants);
		}
	}
	protected virtual void MonitorRange(){
		AddListsTogether();
		if(target == null){
			if(combinedList.Count > 0){
				target = combinedList[0];
				if(Time.time > lastFireStarted + fireRate)
					StartFire();
			}
		}else{
			if(combinedList.IndexOf(target) == -1){
				target = null;
				StopFire();
			}
		}
	}
	
	void Blink(){
		this.renderer.material = blinkMaterial;
		Invoke("ChangeColor", 0.05f);
	}
	
	void ChangeColor(){
		this.renderer.material = defaultMaterial;
	}
	
	protected virtual void Damage(){
		if(target != null){
			EnemyBaseScript enemyScript = target.GetComponent<EnemyBaseScript>();
			enemyScript.Damage(damage);
			Blink();
		}
	}
	
	protected virtual void StartFire(){
		lastFireStarted = Time.time;
		InvokeRepeating("Damage", 0.001f, fireRate);
	}

	protected virtual void StopFire(){
		CancelInvoke("Damage");
	}
	
}
