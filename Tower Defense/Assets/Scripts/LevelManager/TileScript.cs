using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	public Material yellow; //Debug
	protected Material current; //Debug
	protected Material defaultMaterial; //Debug
	protected PathFinderScript pathFinderScript;
	protected LevelScript levelScript;
	public GameObject tileObject;
	public List<GameObject> tileOccupants = new List<GameObject>();

	public void RegisterTileOccupant(GameObject gameObject){
		int occupantCountBefore = tileOccupants.Count;
		//Add tile to the list
		tileOccupants.Add(gameObject);
		if (occupantCountBefore == 0){
			current = this.renderer.material; //Debug
			levelScript.ChangeTileMaterial(this.gameObject, yellow); //Debug
		}
	}
	
	public void RegisterObject(GameObject gameObject){
		tileObject = gameObject;
		PathNode correspondingNode = pathFinderScript.GetNode(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
		correspondingNode.nodeHandicap = 1000;
	}
	public GameObject GetOccupyingObject(){
		return tileObject;
	}
	public bool IsEmpty(){
		if (tileObject == null)
			return true;
		else
			return false;
	}
	
	public bool IsOnPath(){
		bool result = false;
		PathNode tempNode = pathFinderScript.GetNode(this.transform.position.x, this.transform.position.y);
		List<PathNode> tempPath = pathFinderScript.GetPath();
		foreach(PathNode node in tempPath){
			if (node == tempNode)
				result = true;
		}
		return result;
		
	}
	
	public void DeregisterObject(GameObject gameObject){
		tileObject = null;
		PathNode correspondingNode = pathFinderScript.GetNode(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
		correspondingNode.nodeHandicap = 0;		
	}
	
	public int GetOccupantListCount(){
		return tileOccupants.Count;
	}
	
	public void SetDefaultMaterial(Material material){
		defaultMaterial = material;
		current = material;
	}
	
	public void DeregisterTileOccupant(GameObject gameObject){
		//Add tile to the list
		tileOccupants.Remove(gameObject);
		int occupantCountAfter = tileOccupants.Count;
		if (occupantCountAfter == 0){
			levelScript.ChangeTileMaterial(this.gameObject, current); //Debug
		}
	}
	
	public void RevertMaterialToPrefab(){
		this.gameObject.renderer.material = defaultMaterial;
	}
	
	// Use this for initialization
	protected virtual void Start () {
		current = this.renderer.material;
		defaultMaterial = this.renderer.material;
		//Get LevelScript instance
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		//Put the tile in generic list
		pathFinderScript = GameObject.Find("PathFinder(Clone)").GetComponent<PathFinderScript>();
		levelScript.RegisterTile(this.gameObject);
		pathFinderScript.RegisterAsNode(this.gameObject);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}
}
