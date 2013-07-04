using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	public Material yellow; //Debug
	protected Material current; //Debug
	protected PathFinderScript pathFinderScript;
	protected LevelScript levelScript;
	protected List<GameObject> tileOccupants = new List<GameObject>();

	public void RegisterTileOccupant(GameObject gameObject){
		int occupantCountBefore = tileOccupants.Count;
		//Add tile to the list
		tileOccupants.Add(gameObject);
		if (occupantCountBefore == 0){
			current = this.renderer.material; //Debug
			levelScript.ChangeTileMaterial(this.gameObject, yellow); //Debug
		}
	}
	public void DeregisterTileOccupant(GameObject gameObject){
		//Add tile to the list
		tileOccupants.Remove(gameObject);
		int occupantCountAfter = tileOccupants.Count;
		if (occupantCountAfter == 0){
			levelScript.ChangeTileMaterial(this.gameObject, current); //Debug
		}
	}
	
	// Use this for initialization
	protected virtual void Start () {
		current = this.renderer.material;
		//Get LevelScript instance
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		//Put the tile in generic list
		pathFinderScript = GameObject.Find("PathFinder(Clone)").GetComponent<PathFinderScript>();
		levelScript.RegisterTile(this.gameObject);
		pathFinderScript.RegisterAsNode(this.gameObject);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}
}
