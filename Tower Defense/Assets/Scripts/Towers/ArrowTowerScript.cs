using UnityEngine;
using System.Collections;

public class ArrowTowerScript : TowerBaseScript {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		fireRate = 0.2f;
		damage = 2f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	protected override void PopulateRangeWithTiles(){
		float x = this.transform.position.x;
		float y = this.transform.position.y;
		GameObject tile1 = levelScript.GetTile(x,y+1);
		GameObject tile2 = levelScript.GetTile(x+1,y);
		GameObject tile3 = levelScript.GetTile(x,y-1);
		GameObject tile4 = levelScript.GetTile(x-1,y);
		tilesInRange.Add(tile1);
		tilesInRange.Add(tile2);
		tilesInRange.Add(tile3);
		tilesInRange.Add(tile4);
	}
}
