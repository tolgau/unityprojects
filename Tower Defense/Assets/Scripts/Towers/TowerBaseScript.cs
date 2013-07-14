using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TowerBaseScript : MonoBehaviour {
	protected List<GameObject> tilesInRange = new List<GameObject>();
	protected List<GameObject> combinedList = new List<GameObject>();
	protected LevelScript levelScript;
	protected GameObject target;
	protected float fireRate;
	protected float damage;
	
	// Use this for initialization
	protected virtual void Start () {
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
				StartFire();
			}
		}else{
			if(combinedList.IndexOf(target) == -1){
				target = null;
			}
		}
	}
	
	protected virtual void Damage(){
		if(target != null){
			EnemyBaseScript enemyScript = target.GetComponent<EnemyBaseScript>();
			enemyScript.Damage(damage);
		}
		else
			StopFire();
	}
	
	protected virtual void StartFire(){
		InvokeRepeating("Damage", 0f, fireRate);
	}

	protected virtual void StopFire(){
		CancelInvoke();
	}
	
}
