using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {
public GameObject Tile;
//Generic list the tiles are held in
private List<GameObject> tileMap = new List<GameObject>();
	
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
		DrawMap(10,10);
	
	}
	
	//Instantiate according to size	
	void DrawMap(int mapHor, int mapVer){
		//Initiate position vector
		Vector3 positionVector = new Vector3(0,0,0);
			//Start from half of mapVer in order to center the camera
			for(float y=-mapVer/2; y<mapVer/2; y++){
				//Same for mapHor
				for(float x=-mapHor/2; x<mapHor/2; x++){
					positionVector = new Vector3(x,y,0);
					//Rotate -90 degrees Euler for tiles to face the camera
					Instantiate(Tile,positionVector,Quaternion.Euler(-90,0,0));
				}
			}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
