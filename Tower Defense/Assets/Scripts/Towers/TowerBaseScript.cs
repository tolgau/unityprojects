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
	protected float areaDamage;
	protected float projectileSpeed;
	protected float lastFireStarted;
	protected bool firing;
	
	// Use this for initialization
	protected virtual void Start () {
		target = null;
		levelScript = GameObject.Find("LevelManager(Clone)").GetComponent<LevelScript>();
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
				StartCoroutine("StartFire");
			}
		}else{
			if(combinedList.IndexOf(target) == -1){
				target = null;
				StopCoroutine("StartFire");
			}
		}
	}
	
	protected virtual void Launch(){
		if(target != null){
			EnemyBaseScript enemyScript = target.GetComponent<EnemyBaseScript>();
			float [] roundedLoc = enemyScript.GetRoundedLocation();
			GameObject tile = levelScript.GetTile(roundedLoc[0], roundedLoc[1]);
			TileScript tileScript = tile.GetComponent<TileScript>();
			lastFireStarted = Time.time;
			StartCoroutine(DamageRoutine(tileScript, damage, projectileSpeed));
		}
	}
	
	protected virtual IEnumerator DamageRoutine(TileScript tileScript, float damage, float projectileSpeed) {
		Vector3 yNormalized = this.transform.position;
		yNormalized.z = 0f;
		float distance = Vector3.Distance (tileScript.gameObject.transform.position, yNormalized);
		float waitTime = projectileSpeed * distance;
		yield return new WaitForSeconds(waitTime);
		Damage (tileScript);
	}
	
	protected virtual void Damage(TileScript tileScript){
		
	}
	
	protected virtual IEnumerator StartFire(){
		while(true){
			if(Time.time < lastFireStarted + fireRate)
				yield return new WaitForSeconds((lastFireStarted + fireRate) - Time.time);
			Launch();
			yield return new WaitForSeconds(fireRate);
			
		}
	}
}
