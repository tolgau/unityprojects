using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {
	
	public GameObject Tile;
	public GameObject Wall;
	public GameObject Frog;
	public GameObject Border;
	public GameObject GateEnter;
	public GameObject GateExit;
	public GameObject PathFinder;
	public GameObject Debugger;
	public Material wallMat;
	public Material tileMat;
	public int mapWidth;
	public int mapHeight;
	public PathFinderScript pathFinderScript;
	public GameObject tileUnderMouse;
	//Generic list the tiles and walls are held in
	private List<GameObject> tileMap = new List<GameObject>();
	private Quaternion rotation = Quaternion.Euler(-90,0,0);
	public int startPoint;
	public int endPoint;
	
	public void RegisterTile(GameObject tile){
		//Add tile to the list
		tileMap.Add(tile);
	}
	
	//Return the tile object instance at the specified horizontal and vertical values
	public GameObject GetTile(float mapHor, float mapVer){
		GameObject result=null;
		foreach (GameObject tile in tileMap)
		{
		    if(tile.transform.position.x == mapHor && tile.transform.position.y == mapVer)
				result = tile;
		}
		return result;
	}
	
	public void ChangeTileMaterial(GameObject tile, Material material){
		tile.renderer.material = material;		
	}
	
	public void InstantiateFrog(){
		Instantiate(Frog,new Vector3(startPoint,mapHeight/2,-0.5f),Quaternion.Euler(0,0,180));
	}
	// Use this for initialization
	void Start () {
		startPoint = Random.Range(-(mapWidth/2-2), mapWidth/2-2);
		endPoint = Random.Range(-(mapWidth/2-2), mapWidth/2-2);
		BuildMap(mapWidth/2,mapHeight/2);
		GameObject pathFinder = (GameObject)Instantiate(PathFinder);
		InstantiateFrog();
		pathFinderScript = pathFinder.GetComponent<PathFinderScript>();
		Instantiate(Debugger);
	}
	
	//Instantiate according to size	
	void BuildMap(int mapHor, int mapVer){
		
		//Initiate position vector
		Vector3 positionVector = new Vector3(0,0,0);
		Vector3 gatePositionVector = new Vector3(0,0,0);
		//Start from half of mapVer in order to center the camera
		for(float y=-mapVer; y<=mapVer; y++){
			//Same for mapHor
			for(float x=-mapHor; x<=mapHor; x++){

				positionVector = new Vector3(x,y,0);
				gatePositionVector = new Vector3(x,y,-1.1f);
				if (x==mapHor) {
					DrawTiles(Border, positionVector);
				} else if (x==-mapHor) {
					DrawTiles(Border, positionVector);
				} else if (y==mapVer && (x<startPoint || x>startPoint)) {
					DrawTiles(Border, positionVector);
				} else if (y==-mapVer && (x<endPoint || x>endPoint)) {
					DrawTiles(Border, positionVector);
				} else if (y==mapVer && (x == startPoint)){
					DrawTiles(GateEnter, gatePositionVector);
				} else if (y==-mapVer && (x == endPoint)) {
					DrawTiles(GateExit, gatePositionVector);
				} else {
					DrawTiles(Tile, positionVector);
				}				
			}
		}
	}
	
	GameObject DrawTiles(GameObject gameObject, Vector3 positionVector) {
		Instantiate(gameObject, positionVector, rotation);
		return gameObject;
	}
	
	public GameObject GetTileUnderMouse(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int layerMask = 1<<8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
			return hit.transform.gameObject;
		}
		else
			return null;
	}
	
	void PlaceObject(GameObject tile, GameObject placedObject){
		TileScript tempTileScript = tile.GetComponent<TileScript>();
		if(tempTileScript.IsEmpty()){
			GameObject tempObject = (GameObject)Instantiate(placedObject,new Vector3(tile.transform.position.x,tile.transform.position.y,-0.5f),Quaternion.Euler(0,0,180));
			tempTileScript.RegisterObject(tempObject);
		}
		else
			Debug.Log("Tile already occupied!");
	}
	
	void RemoveObject(GameObject tile){
		TileScript tempTileScript = tile.GetComponent<TileScript>();
		if(!tempTileScript.IsEmpty()){
			GameObject tempObject = tempTileScript.GetOccupyingObject();
			tempTileScript.DeregisterObject(tempObject);
			Destroy(tempObject);
		}
		else
			Debug.Log("Tile already free!");
	}
	
	// Update is called once per frame
	void Update () {
		tileUnderMouse = GetTileUnderMouse();
		if (Input.GetButtonDown("Fire1")) {
			PlaceObject(tileUnderMouse, Wall);
			pathFinderScript.FindShortestPathBetweenGates();
		}
		if (Input.GetButtonDown("Fire2")) {
			RemoveObject(tileUnderMouse);
			pathFinderScript.FindShortestPathBetweenGates();
		}
	}	
}
