using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
protected LevelScript levelScript;
protected List<GameObject> tileOccupants = new List<GameObject>();

	public void RegisterTileOccupant(GameObject gameObject){
		//Add tile to the list
		tileOccupants.Add(gameObject);
	}
	public void DeregisterTileOccupant(GameObject gameObject){
		//Add tile to the list
		tileOccupants.Remove(gameObject);
	}
	
	// Use this for initialization
	protected virtual void Start () {
		//Get LevelScript instance
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		//Put the tile in generic list
		levelScript.RegisterTile(this.gameObject);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
}
