using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	public Material yellow; //Debug
	protected Material current; //Debug
	protected Material defaultMaterial; //Debug
	protected PathFinderScript pathFinderScript;
	protected LevelScript levelScript;
	protected float locx;
	protected float locy;
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
	
	public void DamageFirst(float damage){
		if(tileOccupants.Count >= 1){
			GameObject tempTarget = tileOccupants[0];
			EnemyBaseScript tempScript = tempTarget.GetComponent<EnemyBaseScript>();
			tempScript.Damage(damage);
		}
	}
		
	public void DamageAll(float damage){
		DamageAdjacent(damage, damage, 0);
	}
	
	public void DamageAdjacent(float damage, float areaDamage, int sideLength){
		DamageAdjacent(damage,areaDamage, sideLength, false);
	}
	
	public void DamageAdjacent(float damage, float areaDamage, int sideLength, bool boxed){
		List<GameObject> tileList = new List<GameObject>();
		List<GameObject> enemies = new List<GameObject>();
		TileScript baseTileScr = null;
		
		if(boxed){
			for (int x=-sideLength; x<=sideLength; x++) {
				for (int y=-sideLength; y<=sideLength; y++) {
					if (x==0 && y==0)
						baseTileScr = this;
					else {
						if (levelScript.GetTile(locx+x,locy+y).GetComponent<TileScript>().IsOnPath())
							tileList.Add (levelScript.GetTile(locx+x,locy+y));
					}
				}
			}
		}else{
			for (int x=-sideLength; x<=sideLength; x++) {
				for (int y=-sideLength; y<=sideLength; y++) {
					if (x==0 && y==0)
						baseTileScr = this;
					else if (y<=sideLength-Mathf.Abs(x) && y>=Mathf.Abs(x)-sideLength) {
						if (levelScript.GetTile(locx+x,locy+y).GetComponent<TileScript>().IsOnPath())
							tileList.Add (levelScript.GetTile(locx+x,locy+y));
					}
				}
			}
		}
		
		List<GameObject> baseTileOccupants = new List<GameObject>();
			baseTileOccupants.AddRange(baseTileScr.tileOccupants);
		
		foreach(GameObject tile in tileList){
			if(tile != null){
				TileScript tempScr = tile.GetComponent<TileScript>();
				enemies.AddRange(tempScr.tileOccupants);
			}
		}
		
		foreach(GameObject enemy in baseTileOccupants){
			EnemyBaseScript enemyScr = enemy.GetComponent<EnemyBaseScript>();
			enemyScr.Damage(damage);
		}
		
		foreach(GameObject enemy in enemies){
			EnemyBaseScript enemyScr = enemy.GetComponent<EnemyBaseScript>();
			enemyScr.Damage(areaDamage);
		}
		
	}

	public void DamageAdjacent(int upwards, int rightwards, int downwards, int leftwards){
		
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
		locx = this.transform.position.x;
		locy = this.transform.position.y;
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
