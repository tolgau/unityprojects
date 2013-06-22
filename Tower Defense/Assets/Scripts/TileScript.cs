using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {
private LevelScript levelScript;

	
	// Use this for initialization
	void Start () {
		//Get LevelScript instance
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		//Put the tile in generic list
		levelScript.RegisterTile(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
