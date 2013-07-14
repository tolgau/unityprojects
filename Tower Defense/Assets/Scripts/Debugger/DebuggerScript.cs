using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebuggerScript : MonoBehaviour {
	public GUIStyle regularText;
	public GUIStyle titleText;
	protected LevelScript levelScript;
	protected PathFinderScript pathFinderScript;
	protected GameObject currentTile;
	public GameObject DebuggerTile;
	public Texture wallTexture;
	public Texture towerTexture;

	void OnGUI () {
		GUILayout.Label("Tile/Node Info:", titleText);
		string tileLocX = "";
		string tileLocY = "";
		string tileTag = "";
		string tileListCount = "";
		string tilePathOrder = "";
		string handicap = "";
		currentTile = levelScript.tileUnderMouse;
		if (currentTile != null){
			TileScript tileScript = currentTile.GetComponent<TileScript>();
			tileLocX = currentTile.transform.position.x.ToString();
			tileLocY = currentTile.transform.position.y.ToString();
			tileTag = currentTile.tag.ToString();
			int tileListCountI = tileScript.GetOccupantListCount();
			tileListCount = tileListCountI.ToString();
			PathNode tempNode = pathFinderScript.GetNode(currentTile.transform.position.x, currentTile.transform.position.y);
			List<PathNode> path = pathFinderScript.GetPath();
			int tilePathOrderI = path.IndexOf(tempNode);
			tilePathOrder = tilePathOrderI.ToString();
			handicap = tempNode.nodeHandicap.ToString();
			GUILayout.Label("x: " + tileLocX + "\ny: " + tileLocY + "\ntag: " + tileTag + "\noccupants: " + tileListCount + "\npathorder: " + tilePathOrder + "\nhandicap: " + handicap, regularText);
		}
		else
		GUILayout.Label("No tile!", regularText);
		
		GUILayout.Label("\nMob Info:", titleText);
		if (currentTile != null){
			TileScript tileScript = currentTile.GetComponent<TileScript>();
			if(tileScript.GetOccupantListCount() != 0)
			{
				List <GameObject> list = tileScript.tileOccupants;
				foreach(GameObject enemy in list){
					EnemyBaseScript eScript = enemy.GetComponent<EnemyBaseScript>();
					GUILayout.Label("Hit Points: "+ eScript.hitPoints, regularText);
				}
			}
		}
		
        GUILayout.BeginArea (new Rect (Screen.width-120,5,100,300));
		if(GUILayout.Button(wallTexture, GUILayout.Width(35),GUILayout.Height(35)))
			levelScript.cursorObject = levelScript.Wall;
		if(GUILayout.Button(towerTexture, GUILayout.Width(35),GUILayout.Height(35)))
			levelScript.cursorObject = levelScript.ArrowTower;
		GUILayout.Label("- Choose an object to place. \n\n- Right click to delete.");
		if(GUILayout.Button("Start")){
			if(levelScript.spawning == false)
				levelScript.StartLevel();
			else
				Debug.Log("Current level still in progress!");
		}
		if(GUILayout.Button("Quit"))
			Application.Quit();
		GUILayout.EndArea ();		
	}
	
	public void InstantiateDebuggerTile(){
		Instantiate(DebuggerTile,new Vector3(0,0,0),Quaternion.Euler(-90,0,0));
	}
	void Update(){
		
	}
	void Start(){
		InstantiateDebuggerTile();
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		pathFinderScript = levelScript.pathFinderScript;
	}
}