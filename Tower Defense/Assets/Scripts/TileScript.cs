using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {
private LevelScript levelScript;
	
	// Use this for initialization
	void Start () {
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		levelScript.RegisterTile(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
