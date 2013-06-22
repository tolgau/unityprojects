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
	
	public void RegisterTile(GameObject tile){
		//Add tile to the list
		tileMap.Add(tile);
	}
	
	//Return the tile object instance at the specified horizontal and vertical values
	public GameObject GetTile(float mapHor, float mapVer){
        GameObject result = tileMap.Find(
        delegate(GameObject tile)
        {
            return tile.transform.position.x == mapHor && tile.transform.position.y == mapVer;
        }
        );
		return result;
	}
	
	// Use this for initialization
	void Start () {
		int width = 10;
		int height = 10;
		DrawMap(width,height);
		Instantiate(Frog,new Vector3(0,0,-0.5f),Quaternion.Euler(0,0,180));
	}
	
	//Instantiate according to size	
	void DrawMap(int mapHor, int mapVer){
		DrawTile(mapHor,mapVer);
		DrawWall(mapHor,mapVer);
	}
	
	void DrawTile(int mapHor, int mapVer) {
		//Initiate tile vector
		Vector3 tileVector = new Vector3(0,0,0);
		
		for(float y=-(mapVer-1)/2; y<=(mapVer-1)/2; y++){
			//Same for mapHor
			for(float x=-(mapHor-1)/2; x<=(mapHor-1)/2; x++){
				tileVector = new Vector3(x,y,0);
				
				//Rotate -90 degrees Euler for tiles to face the camera
				Instantiate(Tile,tileVector,rotation);
			}
		}
	}
	
	void DrawWall(int mapHor, int mapVer) {
		int startPoint = Random.Range(-((mapHor-1)/2-mapHor/10), ((mapHor-1)/2-mapHor/10));
		int endPoint = Random.Range(-((mapHor-1)/2-mapHor/10), ((mapHor-1)/2-mapHor/10));
		
		//Initiate wall vector
		Vector3 wallVector = new Vector3((mapHor+1)/2,(mapVer+1)/2,0);
		
		Instantiate(Wall,wallVector,rotation);
		wallVector = new Vector3((mapHor+1)/2,(-mapVer-1)/2,0);
		Instantiate(Wall,wallVector,rotation);
		wallVector = new Vector3((-mapHor-1)/2,(mapVer+1)/2,0);
		Instantiate(Wall,wallVector,rotation);
		wallVector = new Vector3((-mapHor-1)/2,(-mapVer-1)/2,0);
		Instantiate(Wall,wallVector,rotation);
		
		//Start from half of mapVer in order to center the camera
		for(float y=-(mapVer-1)/2; y<=(mapVer-1)/2; y++){
			//Same for mapHor
			for(float x=-(mapHor-1)/2; x<=(mapHor-1)/2; x++){
				
				if(x==-(mapHor-1)/2) {
					wallVector = new Vector3(x-1,y,0);
					Instantiate(Wall,wallVector,rotation);
				}
				
				if(x==(mapHor-1)/2) {
					wallVector = new Vector3(x+1,y,0);
					Instantiate(Wall,wallVector,rotation);
				}
			
				if(y==-(mapVer-1)/2 && x<startPoint-1 || x>startPoint+1) {
					wallVector = new Vector3(x,y-1,0);
					Instantiate(Wall,wallVector,rotation);
				}
				
				if(y==(mapVer-1)/2 && x<endPoint-1 || x>endPoint+1) {
					wallVector = new Vector3(x,y+1,0);
					Instantiate(Wall,wallVector,rotation);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
