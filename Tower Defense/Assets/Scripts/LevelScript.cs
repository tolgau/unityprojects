using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour {
public GameObject Tile;
	// Use this for initialization
	void Start () {
		DrawMap(10,10);
	
	}
	//Instantiate according to size	
	void DrawMap(int mapHor, int mapVer){
		GameObject[,] tileMap;
		tileMap = new GameObject*[mapHor,mapVer];
		//Initiate position vector
		Vector3 positionVector = new Vector3(0,0,0);
			//Start from half of mapVer in order to center the camera
			for(float y=-mapVer/2; y<mapVer/2; y++){
				//Same for mapHor
				for(float x=-mapHor/2; x<mapHor/2; x++){
					positionVector = new Vector3(x,y,0);
					//Rotate -90 degrees Euler for tiles to face the camera
					GameObject* tempPointer;
					tempPointer = &Instantiate(Tile,positionVector,Quaternion.Euler(-90,0,0));
					
				}
			}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
