using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {
	
	public GameObject Tile;
	public GameObject Frog;
	public GameObject Wall;
	public GameObject Border;
	public GameObject GateEnter;
	public GameObject GateExit;
	public GameObject PathFinder;
	public Material wallMat;
	public Material tileMat;
	public int mapWidth;
	public int mapHeight;
	public PathFinderScript pathFinderScript;
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
	
	// Use this for initialization
	void Start () {
		startPoint = Random.Range(-(mapWidth/2-2), mapWidth/2-2);
		endPoint = Random.Range(-(mapWidth/2-2), mapWidth/2-2);
		BuildMap(mapWidth/2,mapHeight/2);
		GameObject pathFinder = (GameObject)Instantiate(PathFinder);
		Instantiate(Frog,new Vector3(startPoint,mapHeight/2,-0.5f),Quaternion.Euler(0,0,180));
		pathFinderScript = pathFinder.GetComponent<PathFinderScript>();
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
					DrawObject(Border, positionVector);
				} else if (x==-mapHor) {
					DrawObject(Border, positionVector);
				} else if (y==mapVer && (x<startPoint || x>startPoint)) {
					DrawObject(Border, positionVector);
				} else if (y==-mapVer && (x<endPoint || x>endPoint)) {
					DrawObject(Border, positionVector);
				//} else if ( y==0 && (x<12 && x > -12)) {
				//	DrawObject(Wall, positionVector);
				} else if (y==mapVer && (x == startPoint)){
					DrawObject(GateEnter, gatePositionVector);
				} else if (y==-mapVer && (x == endPoint)) {
					DrawObject(GateExit, gatePositionVector);
				} else {
					DrawObject(Tile, positionVector);
				}				
			}
		}
	}
	
	GameObject DrawObject(GameObject gameObject, Vector3 positionVector) {
		
		Instantiate(gameObject, positionVector, rotation);
		return gameObject;
	}
	
	GameObject GetTileUnderMouse(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int layerMask = 1<<8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
			return hit.transform.gameObject;
		}
		else
			return null;
	}
	
	void TileWallCycle(GameObject tile){
		if (tile.tag == "Tile"){
			tile.tag = "Wall";
			tile.renderer.material = wallMat;
			TileScript tempTileScript = tile.GetComponent<TileScript>();
			tempTileScript.SetDefaultMaterial(wallMat);
			float mapHor = tile.transform.position.x;
			float mapVer = tile.transform.position.y;
			PathNode tempNode = pathFinderScript.GetNode(mapHor, mapVer);
			tempNode.nodeHandicap = 1000;
		} else if (tile.tag == "Wall"){
			tile.tag = "Tile";
			tile.renderer.material = tileMat;
			TileScript tempTileScript = tile.GetComponent<TileScript>();
			tempTileScript.SetDefaultMaterial(tileMat);
			float mapHor = tile.transform.position.x;
			float mapVer = tile.transform.position.y;
			PathNode tempNode = pathFinderScript.GetNode(mapHor, mapVer);
			tempNode.nodeHandicap = 0;
		}
		else
			Debug.Log("Please select either a tile or a wall.");

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			GameObject hit = GetTileUnderMouse();
			TileWallCycle (hit);
			pathFinderScript.FindShortestPathBetweenGates();
		}
	}	
}
