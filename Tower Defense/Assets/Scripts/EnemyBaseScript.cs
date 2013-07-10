using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBaseScript : MonoBehaviour {
	
	protected PathFinderScript pathFinderScript;
	protected LevelScript levelScript;
	protected float speed=0.1f;
	protected float hitPoints;
	protected int pathCount;
	protected GameObject currentTile;
	protected List<PathNode> path;
	// Use this for initialization
	protected virtual void Start ()
	{
		//Get LevelScript instance
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		pathFinderScript = GameObject.Find("PathFinder(Clone)").GetComponent<PathFinderScript>();
		path = pathFinderScript.GetPath();
		pathCount = path.Count;
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
		
		if (pathCount != 0) {
			Vector3 pathPosition = path[pathCount-1].nodePosition;

			if (pathPosition.x == currentTile.transform.position.x && pathPosition.y == currentTile.transform.position.y)
				pathCount--;
			
			transform.Translate((currentTile.transform.position.x-pathPosition.x)*speed,(currentTile.transform.position.y-pathPosition.y)*speed,0);
		} else {
			if (levelScript.GetTile(currentTile.transform.position.x, currentTile.transform.position.y).tag == "GateExit") {
				DestroyEnemy();
				levelScript.InstantriateFrog();
			} else {
				transform.Translate(0,speed,0);	
			}
		}
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
		Move ();
		MonitorTileChange();
		transform.Translate (new Vector3 (-Input.GetAxis ("Horizontal") * Time.deltaTime * 5, 0, 0));
		transform.Translate (new Vector3 (0, -Input.GetAxis ("Vertical") * Time.deltaTime * 5, 0));
		if(Input.GetKeyDown("space")){
			GameObject currentTempTile = GetCurrentTile();
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
	
	protected void DestroyEnemy(){
		DeregisterOccupant(currentTile);
		Destroy (this.gameObject);
	}
	
	protected virtual bool RegisterOccupant (GameObject tile)
	{
		if(tile != null){
			tile.GetComponent<TileScript>().RegisterTileOccupant(this.gameObject);
			return true;
		}
		else
			return false;
	}
	
	protected virtual bool DeregisterOccupant (GameObject tile)
	{
		if(tile != null){
			tile.GetComponent<TileScript>().DeregisterTileOccupant(this.gameObject);
			return true;
		}
		else
			return false;
	}
	
}