using UnityEngine;
using System.Collections;

public abstract class EnemyScript : MonoBehaviour {
protected LevelScript levelScript;
protected float speed;
protected float hitPoints;
	
	// Use this for initialization
	protected virtual void Start () {
		//Get LevelScript instance
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		//Move according to axis information taken from keyboard
		transform.Translate(new Vector3(-Input.GetAxis("Horizontal")*Time.deltaTime*2,0,0));
		transform.Translate(new Vector3(0,-Input.GetAxis("Vertical")*Time.deltaTime*2,0));
		
		//For debug purposes. Get the tile the frog is sitting on and print out the coordinates everytime space is pressed
		if(Input.GetButtonDown("Jump")){
		//Get frog coordinates and round them. Send integer values although the method expects float
		float tempHorizontal = Mathf.Round(transform.position.x);
		float tempVertical = Mathf.Round(transform.position.y);
		//Get corresponding tile
		GameObject tempTile = levelScript.GetTile(tempHorizontal,tempVertical);
			if(tempTile == null)
			Debug.Log("There is no tile in that location.");
			else
			Debug.Log(tempTile.transform.position.x + " " + tempTile.transform.position.y);
				
		//tempTile.transform.position.z = 3f;
		}
	}
	
}