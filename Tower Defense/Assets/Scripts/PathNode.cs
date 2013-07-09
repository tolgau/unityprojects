using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode {
	public Vector3 nodePosition;
	public float nodeF, nodeG, nodeH;
	public float nodeHandicap;
	public PathNode parentNode;
	public bool opened;
	public bool closed;
	protected PathFinderScript pathFinderScript;
	
	public PathNode(){
		//Get LevelScript instance
		pathFinderScript = GameObject.Find("PathFinder(Clone)").GetComponent<PathFinderScript>();
		//Put the tile in generic list
		//pathFinderScript.RegisterNode(this);
		//pathFinderScript.PrintNodeList();
	}
}
