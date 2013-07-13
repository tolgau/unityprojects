using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBaseScript : MonoBehaviour {
	
	protected PathFinderScript pathFinderScript;
	protected LevelScript levelScript;
	protected float speed=0.1f;
	protected float placementError = 0.001f;
	protected float hitPoints;
	protected int pathCount;
	protected GameObject currentTile, nextTile, previousTile;
	protected List<PathNode> path;
	// Use this for initialization
	protected virtual void Start ()
	{
		//Get LevelScript instance
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		pathFinderScript = GameObject.Find("PathFinder(Clone)").GetComponent<PathFinderScript>();
		path = pathFinderScript.GetPath();
		nextTile = levelScript.GetTile(path[path.Count-1].nodePosition.x, path[path.Count-1].nodePosition.y);
		previousTile = levelScript.GetTile(this.transform.position.x, this.transform.position.y);
		currentTile = GetCurrentTile();
		RegisterOccupant(currentTile);
	}
	
	protected virtual float[] GetRoundedLocation() {
		//Get frog coordinates and round them. Send integer values although the method expects float
		float tempHorizontal = Mathf.Round (transform.position.x);
		float tempVertical = Mathf.Round (transform.position.y);
		float[] roundedLocation = new float [] {tempHorizontal, tempVertical};
		return roundedLocation;
	}
	
	protected virtual GameObject GetCurrentTile() {
		float[] tempLocation = GetRoundedLocation();
		currentTile = levelScript.GetTile(tempLocation[0], tempLocation[1]);
		return currentTile;
	}
	
	protected virtual int FindinPathList(GameObject tile) {
		int i = 0;
		foreach(PathNode node in path) {
			if (node.nodePosition.x == tile.transform.position.x && node.nodePosition.y == tile.transform.position.y)
				return i;
			i++;
		}
		return -1;
	}
	
	protected virtual void GetNextTile() {
		int i = FindinPathList(previousTile);
		
		if (i != -1 && i != 0)
			nextTile = levelScript.GetTile(path[i-1].nodePosition.x, path[i-1].nodePosition.y);
		else
			nextTile = levelScript.GetTile(path[0].nodePosition.x, path[0].nodePosition.y-1);
	}
	
	protected virtual void Move(){

		if (Mathf.Abs(this.transform.position.x - nextTile.transform.position.x) < speed && Mathf.Abs(this.transform.position.y - nextTile.transform.position.y) < speed){
			previousTile = nextTile;
			
			GetNextTile();
			
			if (previousTile.tag == "GateExit") {
				DestroyEnemy();
			}
		}
		
		transform.Translate((previousTile.transform.position.x-nextTile.transform.position.x)*speed,(previousTile.transform.position.y-nextTile.transform.position.y)*speed,0);
			
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