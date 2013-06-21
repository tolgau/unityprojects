using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
private LevelScript levelScript;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(-Input.GetAxis("Horizontal")*Time.deltaTime*10,0,0));
		transform.Translate(new Vector3(0,-Input.GetAxis("Vertical")*Time.deltaTime*10,0));
		
		if(Input.GetButtonDown("Jump")){
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		GameObject tempTile = levelScript.GetTile(100f,2f);
			if(tempTile == null)
			Debug.Log("There is no tile in that location.");
		//tempTile.transform.position.z = 3f;
		}
	}
	
}