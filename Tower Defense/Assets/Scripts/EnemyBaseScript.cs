using UnityEngine;
using System.Collections;

public abstract class EnemyBaseScript : MonoBehaviour {
	
	protected LevelScript levelScript;
	protected float speed;
	protected float hitPoints;
	protected GameObject currentTile;
	// Use this for initialization
	protected virtual void Start ()
	{
		//Get LevelScript instance
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();		
	}
	
	protected virtual float[] GetRoundedLocation(){
		//Get frog coordinates and round them. Send integer values although the method expects float
		float tempHorizontal = Mathf.Round (transform.position.x);
		float tempVertical = Mathf.Round (transform.position.y);
		float[] roundedLocation = new float [] {tempHorizontal, tempVertical};
		return roundedLocation;
	}
	
	protected virtual GameObject GetCurrentTile(){
		float[] tempLocation = GetRoundedLocation();
		currentTile = levelScript.GetTile(tempLocation[0], tempLocation[1]);
		return currentTile;
	}
	
	protected virtual bool MonitorTileChange(){
		GameObject tempTile = currentTile;
		GetCurrentTile();
		if(tempTile != currentTile)
			return Registrar(currentTile, tempTile);
		else
			return false;
	}
	// Update is called once per frame
	protected virtual void Update ()
	{
		//Move according to axis information taken from keyboard
		int startPoint = levelScript.startPoint;
		int endPoint = levelScript.endPoint;
		/*
		if (transform.position.x > endPoint ) {
			Vector3 newPosition = new Vector3(transform.position.x-1, transform.position.y-1 , transform.position.z);
			transform.position = Vector3.Lerp(transform.position,newPosition,Time.deltaTime*2);
		} else if (transform.position.x < endPoint ) {
			Vector3 newPosition = new Vector3(transform.position.x+1, transform.position.y-1 , transform.position.z);
			transform.position = Vector3.Lerp(transform.position,newPosition,Time.deltaTime*2);
		} else {
			Vector3 newPosition = new Vector3(transform.position.x, transform.position.y-1 , transform.position.z);
			transform.position = Vector3.Lerp(transform.position,newPosition,Time.deltaTime*2);
		}
		*/
		transform.Translate (new Vector3 (-Input.GetAxis ("Horizontal") * Time.deltaTime * 2, 0, 0));
		transform.Translate (new Vector3 (0, -Input.GetAxis ("Vertical") * Time.deltaTime * 2, 0));
		
	}
	
	protected virtual bool Registrar(GameObject tileEntered, GameObject tileLeft){
		if(DeregisterOccupant(tileLeft) && RegisterOccupant(tileEntered))
			return true;
		else
		{
			Debug.Log("Object registration failed!!!");
			return false;
		}
	}
	
	protected virtual bool RegisterOccupant (GameObject tile)
	{
		tile.GetComponent<TileScript>().RegisterTileOccupant(this.gameObject);
		return true;		
	}
	
	protected virtual bool DeregisterOccupant (GameObject tile)
	{
		tile.GetComponent<TileScript>().DeregisterTileOccupant(this.gameObject);
		return true;
	}
	
}