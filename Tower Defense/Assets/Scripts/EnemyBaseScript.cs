using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBaseScript : MonoBehaviour {
	
	protected PathFinderScript pathFinderScript;
	protected LevelScript levelScript;
	protected float speed;
	protected float hitPoints;
	protected GameObject currentTile;
	protected List<PathNode> path;
	// Use this for initialization
	protected virtual void Start ()
	{
		//Get LevelScript instance
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		pathFinderScript = GameObject.Find("PathFinder(Clone)").GetComponent<PathFinderScript>();
		path = pathFinderScript.GetPath();
		currentTile = GetCurrentTile();
		RegisterOccupant(currentTile);
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
	
	protected virtual void Move(){
		Vector3 pathPosition = path[0].nodePosition;
		this.transform.position = Vector3.Lerp(this.gameObject.transform.position, pathPosition, 0.007f);
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
		//Move ();
		MonitorTileChange();
		transform.Translate (new Vector3 (-Input.GetAxis ("Horizontal") * Time.deltaTime * 5, 0, 0));
		transform.Translate (new Vector3 (0, -Input.GetAxis ("Vertical") * Time.deltaTime * 5, 0));
		if(Input.GetKeyDown("space")){
			GameObject currentTempTile = GetCurrentTile();
 			Debug.Log(currentTempTile.transform.position.x + " " + currentTempTile.transform.position.y + " " + currentTempTile.tag);
		}
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