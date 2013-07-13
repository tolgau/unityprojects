using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode {
	public Vector3 nodePosition;
	public float nodeF, nodeG, nodeH;
	public float nodeHandicap;
	public PathNode parentNode;
	//public bool opened;
	//public bool closed;
	protected PathFinderScript pathFinderScript;
	
	public PathNode(Vector3 nodePos){
		nodePosition = nodePos;
	}
	
	public PathNode(Vector3 nodePos, float nodeHand){
		nodePosition = nodePos;
		nodeHandicap = nodeHand;
	}
	
	public PathNode(){

	}
}
