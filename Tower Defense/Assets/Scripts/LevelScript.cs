using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {
	
	public GameObject Tile;
	public GameObject Frog;
	public GameObject Wall;
	//Generic list the tiles and walls are held in
	private List<GameObject> tileMap = new List<GameObject>();
	private Quaternion rotation = Quaternion.Euler(-90,0,0);
	public int startPoint;
	public int endPoint;
	
	public void RegisterTile(GameObject tile){
		//Add tile to the list
		Debug.Log(tile.transform.position.x + "  " + tile.transform.position.y + " " + tile.GetComponent<TileBaseScript>().tileType);
		tileMap.Add(tile);
		var item = tileMap[tileMap.Count - 1];	
		Debug.Log(item.transform.position.x + "  " + item.transform.position.y + " " + item.GetComponent<TileBaseScript>().tileType);
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
	
	// Use this for initialization
	void Start () {
		int width = 18;
		int height = 10;
		startPoint = Random.Range(-(width/2-3), width/2-3);
		endPoint = Random.Range(-(width/2-3), width/2-3);
		DrawMap(width/2,height/2);
		Instantiate(Frog,new Vector3(startPoint,height/2,-0.5f),Quaternion.Euler(0,0,180));
	}
	
	//Instantiate according to size	
	void DrawMap(int mapHor, int mapVer){
		DrawTile(mapHor, mapVer);
	}
	
	void DrawTile(int mapHor, int mapVer) {
		
		//Initiate position vector
		Vector3 positionVector = new Vector3(0,0,0);
		
		//Start from half of mapVer in order to center the camera
		for(float y=-mapVer; y<=mapVer; y++){
			//Same for mapHor
			for(float x=-mapHor; x<=mapHor; x++){

				positionVector = new Vector3(x,y,0);
				
				if(x==mapHor) {
					Instantiate(Wall,positionVector,rotation);
				} else if(x==-mapHor) {
					Instantiate(Wall,positionVector,rotation);
				} else if(y==mapVer && (x<startPoint-1 || x>startPoint+1)) {
					Instantiate(Wall,positionVector,rotation);
				} else if(y==-mapVer && (x<endPoint-1 || x>endPoint+1)) {
					Instantiate(Wall,positionVector,rotation);
				} else {
					Instantiate(Tile,positionVector,rotation);
				}				
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
