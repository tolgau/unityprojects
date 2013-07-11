using UnityEngine;
using System.Collections;

public class DebuggerTileScript : MonoBehaviour {
	protected LevelScript levelScript;

	// Use this for initialization
	void Start () {
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(levelScript.tileUnderMouse != null){
			this.renderer.enabled = true;
			Vector3 translateVector = new Vector3(levelScript.tileUnderMouse.transform.position.x, levelScript.tileUnderMouse.transform.position.y, -2f);
			this.gameObject.transform.position = translateVector;
		}
		else{
			this.renderer.enabled = false;
		}
	}
}
