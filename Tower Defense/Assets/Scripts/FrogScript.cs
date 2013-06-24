using UnityEngine;
using System.Collections;

public class FrogScript : EnemyBaseScript {

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(Input.GetKeyDown("space")){
			GameObject currentTempTile = GetCurrentTile();
 			Debug.Log(currentTempTile.transform.position.x + " " + currentTempTile.transform.position.y + " " + currentTempTile.GetComponent<TileScript>().GetTileType());
		}
		base.Update();
	
	}
}
