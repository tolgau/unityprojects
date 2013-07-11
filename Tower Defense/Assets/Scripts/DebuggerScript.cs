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

	void OnGUI () {
		GUILayout.TextArea("Tile Info:", titleText);
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
			GUILayout.TextArea("x: " + tileLocX + "\ny: " + tileLocY + "\ntag: " + tileTag + "\noccupants: " + tileListCount + "\npathorder: " + tilePathOrder + "\nhandicap: " + handicap, regularText);
		}
		else
		GUILayout.TextArea("No tile!", regularText);
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