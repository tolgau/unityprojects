using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Node {
	public Vector3 nodePosition;
	public float nodeF, nodeG, nodeH;
	public float nodeHandicap;
	public Node parentNode;
}

public class PathFinderScript : MonoBehaviour {
	
	private List<Node> closedList = new List<Node>();
	private List<Node> openList = new List<Node>();
	private List<Node> path = new List<Node>();
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void FindShortestPathBetweenGates(){
		GameObject[] gateExit = GameObject.FindGameObjectsWithTag("GateExit");
		GameObject[] gateEnter = GameObject.FindGameObjectsWithTag("GateEnter");
		Node start;
		Node end;
		start.nodePosition = gateEnter[0].transform.position;
		start.nodePosition.y--;
		end.nodePosition = gateExit[0].transform.position;
		end.nodePosition.y++;
		FindShortestPathBetween(start, end);
	}
	
	void FindShortestPathBetween(Node start, Node end){

	}
	
}
