using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TileBaseScript : MonoBehaviour {
protected LevelScript levelScript;
protected List<GameObject> tileOccupants = new List<GameObject>();
public TileType tileType;	
	
	public enum TileType {
		tile = 0,
		wall,
		gateEnter,
		gateExit
	}
	public TileType GetTileType(){
		TileType ret = this.tileType;
		return ret;		
	}
	
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
