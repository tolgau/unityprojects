using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {
	
	public GameObject Tile;
	public GameObject Frog;
	public GameObject Wall;
	public GameObject GateEnter;
	public GameObject GateExit;
	public GameObject PathFinder;
	public int mapWidth;
	public int mapHeight;
	
	
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
        /*GameObject result = tileMap.Find(
        delegate(GameObject tile)
        {
            return tile.transform.position.x == mapHor && tile.transform.position.y == mapVer;
        }
        );
        */
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
		Instantiate(Frog,new Vector3(startPoint,mapHeight/2,-0.5f),Quaternion.Euler(0,0,180));
		GameObject pathFinder = (GameObject)Instantiate(PathFinder);
	}
	
	//Instantiate according to size	
	void BuildMap(int mapHor, int mapVer){
		
		//Initiate position vector
		Vector3 positionVector = new Vector3(0,0,0);
		
		//Start from half of mapVer in order to center the camera
		for(float y=-mapVer; y<=mapVer; y++){
			//Same for mapHor
			for(float x=-mapHor; x<=mapHor; x++){

				positionVector = new Vector3(x,y,0);
				
				if (x==mapHor) {
					DrawObject(Wall, positionVector);
				} else if (x==-mapHor) {
					DrawObject(Wall, positionVector);
				} else if (y==mapVer && (x<startPoint || x>startPoint)) {
					DrawObject(Wall, positionVector);
				} else if (y==-mapVer && (x<endPoint || x>endPoint)) {
					DrawObject(Wall, positionVector);
				} else if (y==mapVer && (x == startPoint)){
					DrawObject(GateEnter, positionVector);
				} else if (y==-mapVer && (x == endPoint)) {
					DrawObject(GateExit, positionVector);
				} else {
					DrawObject(Tile, positionVector);
				}				
			}
		}
	}
	
	void DrawObject(GameObject gameObject, Vector3 positionVector) {
		
		Instantiate(gameObject, positionVector, rotation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
