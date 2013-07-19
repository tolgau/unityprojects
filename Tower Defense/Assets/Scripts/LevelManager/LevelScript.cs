using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {
	//Objects
	public GameObject ArrowTower;
	public GameObject Wall;
	public GameObject Frog;
	//END: Objects
	
	//Level
	public GameObject Tile;
	public GameObject Border;
	public GameObject GateEnter;
	public GameObject GateExit;
	//END: Level
	
	//Managers
	public GameObject PathFinder;
	public GameObject Debugger;
	//END: Managers
	
	//Spawner
	public bool spawning;
	public float spawnRate;
	public float spawnStart = 0f;
	public float spawnTimes = 0f;
	public int spawnIndex;
	public GameObject spawnObject;
	public List<SpawnData> spawnQueue = new List<SpawnData>();
	//END: Spawner
	
	public Material wallMat;
	public Material tileMat;
	public int mapWidth;
	public int mapHeight;
	public PathFinderScript pathFinderScript;
	public GameObject tileUnderMouse;
	public GameObject cursorObject;
	//Generic list the tiles and walls are held in
	private List<GameObject> tileMap = new List<GameObject>();
	private Quaternion rotation = Quaternion.Euler(0,0,0);
	public int startPoint;
	public int endPoint;
	
	public void RegisterTile(GameObject tile){
		//Add tile to the list
		tileMap.Add(tile);
	}
	
	public void ChangeSpeed(float speed){
		Time.timeScale = speed;		
	}
	
	public float GetSpeed(){
		return Time.timeScale;
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

	// Use this for initialization
	void Start () {
		spawnRate = 0.5f;
		cursorObject = ArrowTower;
		startPoint = Random.Range(-(mapWidth/2-2), mapWidth/2-2);
		endPoint = Random.Range(-(mapWidth/2-2), mapWidth/2-2);
		BuildMap(mapWidth/2,mapHeight/2);
		GameObject pathFinder = (GameObject)Instantiate(PathFinder);
		pathFinderScript = pathFinder.GetComponent<PathFinderScript>();
		Instantiate(Debugger);
		spawnQueue.Add(new SpawnData(Frog, 10f));
		spawnQueue.Add(new SpawnData(Frog, 2f));
		spawning = false;
	}
	
	public void StartLevel(){
		spawnIndex = -1;
		SpawnNext();
		spawning = true;
	}
	
	public void SpawnNext(){
		spawnIndex++;
		if(spawnIndex < spawnQueue.Count){
			GameObject spawnObject = spawnQueue[spawnIndex].spawnObject;
			float spawnTimes = spawnQueue[spawnIndex].spawnTimes;
			SpawnObjectTimes(spawnObject, spawnTimes);
		}else{
			spawning = false;
		}
	}
	
	public void InstantiateAtStart(GameObject iObject){
		Instantiate(iObject,new Vector3(startPoint,mapHeight/2,-0.5f),Quaternion.Euler(0,0,180));
	}
	
	public void SpawnInstantiate(){
        if (spawnStart + spawnTimes * spawnRate + spawnRate-0.01f < Time.time){
            CancelInvoke("SpawnInstantiate");
			SpawnNext();
		}
		else
			InstantiateAtStart(spawnObject);
    }
	
	public void SpawnObjectTimes(GameObject sObject, float times){
		spawnStart = Time.time;
		spawnTimes = times;
		spawnObject = sObject;
		InvokeRepeating("SpawnInstantiate",spawnRate,spawnRate);

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
		if(tile != null){
			TileScript tempTileScript = tile.GetComponent<TileScript>();
			if(tempTileScript.IsEmpty()){
				GameObject tempObject = (GameObject)Instantiate(placedObject,new Vector3(tile.transform.position.x,tile.transform.position.y,-0.5f),Quaternion.Euler(0,0,180));
				tempTileScript.RegisterObject(tempObject);
			}
			else
				Debug.Log("Tile already occupied!");
			
			if(tempTileScript.IsOnPath())
				pathFinderScript.FindShortestPathBetweenGates();
		}
	}
	
	void RemoveObject(GameObject tile){
		if(tile != null){
			TileScript tempTileScript = tile.GetComponent<TileScript>();
			if(!tempTileScript.IsEmpty()){
				GameObject tempObject = tempTileScript.GetOccupyingObject();
				tempTileScript.DeregisterObject(tempObject);
				Destroy(tempObject);
			}
			else
				Debug.Log("Tile already free!");
			
			pathFinderScript.FindShortestPathBetweenGates();
		}
	}
	
	// Update is called once per frame
	void Update () {
		tileUnderMouse = GetTileUnderMouse();
		if(tileUnderMouse != null){
			if (Input.GetButtonDown("Fire1")) {
				PlaceObject(tileUnderMouse, cursorObject);
			}
			if (Input.GetButtonDown("Fire2")) {
				RemoveObject(tileUnderMouse);
			}
		}
	}	
}
