using UnityEngine;
using System.Collections;

public class TileScript : TileBaseScript {

	// Use this for initialization
	protected override void Start () {
		tileType = TileBaseScript.TileType.tile;
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
}
