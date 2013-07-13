using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TowerBaseScript : MonoBehaviour {
	protected List<GameObject> tilesInRange = new List<GameObject>();
	protected GameObject target;
	protected float fireRate;
	protected float damage;
	
	// Use this for initialization
	protected virtual void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
	
	protected virtual void PopulateRangeWithTiles(){
		
	}
	
	protected virtual void MonitorRange(){
		
	}
	
	protected virtual void Fire(){
		
	}
}
