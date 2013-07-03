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
	private LevelScript levelScript;
	private List<Node> closedList = new List<Node>();
	private List<Node> openList = new List<Node>();
	private List<Node> path = new List<Node>();
	public Material blue;
	public Material red;
	
	// Use this for initialization
	void Start () {
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		FindShortestPathBetweenGates();
	}
	
	public void FindShortestPathBetweenGates(){
		GameObject[] gateExit = GameObject.FindGameObjectsWithTag("GateExit");
		GameObject[] gateEnter = GameObject.FindGameObjectsWithTag("GateEnter");
		Node start = new Node();
		Node end = new Node();
		start.nodePosition = gateEnter[0].transform.position;
		start.nodePosition.y--;
		end.nodePosition = gateExit[0].transform.position;
		end.nodePosition.y++;
		FindShortestPathBetween(start, end);
		PaintOpenListNodes();
		PaintClosedListNodes();
	}
	
	void FindShortestPathBetween(Node start, Node end){		
		bool isClosedList=false, isOpenList=false;
		
		openList.Add (start);
		//closedList.Add (start);
		CalculateF(start, end, openList);
		
		//while(closedList[closedList.Count - 1].nodePosition != end.nodePosition) {
			
			Node nodeWithLowestFValue = FindLowestFValue(openList); 
			if (nodeWithLowestFValue == end) {
				// this node is the goal then we're done
			} else {
				closedList.Add (nodeWithLowestFValue);
				openList.Remove (nodeWithLowestFValue);
			
				List<Node> neighborNodesList = new List<Node>();
				Node neighborNode1 = new Node();
				neighborNode1.nodePosition = nodeWithLowestFValue.nodePosition;
				
				neighborNode1.nodePosition.x = nodeWithLowestFValue.nodePosition.x+1;
				neighborNodesList.Add (neighborNode1);
			
				Node neighborNode2 = new Node();
				neighborNode2.nodePosition = nodeWithLowestFValue.nodePosition;
				neighborNode2.nodePosition.x = nodeWithLowestFValue.nodePosition.x-1;
				neighborNodesList.Add (neighborNode2);
			
				Node neighborNode3 = new Node();
				neighborNode3.nodePosition = nodeWithLowestFValue.nodePosition;
				neighborNode3.nodePosition.y = nodeWithLowestFValue.nodePosition.y+1;
				neighborNodesList.Add (neighborNode3);
			
				Node neighborNode4 = new Node();
				neighborNode4.nodePosition = nodeWithLowestFValue.nodePosition;
				neighborNode4.nodePosition.y = nodeWithLowestFValue.nodePosition.y-1;
				neighborNodesList.Add (neighborNode4);				

				foreach (Node neighbor in neighborNodesList) {
					foreach (Node node in openList) {
						if (node.nodePosition == neighbor.nodePosition) {
							isOpenList = true;
						} else {
							isOpenList = false;
						}
					}
				
					foreach (Node node in closedList) {
						if (node.nodePosition == neighbor.nodePosition) {
							isClosedList = true;
						} else {
							isClosedList = false;
						}
					}
				

	            	if (isClosedList && (nodeWithLowestFValue.nodeG < neighbor.nodeG)) {
				//		update the neighbor with the new, lower, g value 
				//		change the neighbor's parent to our current node
					}
					else if (isOpenList && (nodeWithLowestFValue.nodeG < neighbor.nodeG)) {
				//		update the neighbor with the new, lower, g value 
				//		change the neighbor's parent to our current node
					}
					else if ( !isClosedList && !isOpenList) {
						openList.Add (neighbor);
						CalculateF(start, end, openList);
					}
				}
				
			}
		//}
	}
	
	void CalculateF(Node start, Node end, List<Node> openList) {
		
		foreach (Node node in openList) {
			node.nodeG = Mathf.Sqrt(Mathf.Pow((start.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((start.nodePosition.y - node.nodePosition.y), 2));
			node.nodeH = Mathf.Sqrt(Mathf.Pow((start.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((start.nodePosition.y - node.nodePosition.y), 2));
			node.nodeF = node.nodeH + node.nodeG;
		}
	}
	
	Node FindLowestFValue(List<Node> openList) {
		int lowest = 0;
		for (int i = 1; i < openList.Count; ++i)
		{
			if (openList[i].nodeF < openList[lowest].nodeF)
			{
			    lowest = i;
			}
		}
		return openList[lowest];
	}
	
	GameObject FindTileByNode(Node node){
		float horizontal = node.nodePosition.x;
		float vertical = node.nodePosition.y;
		GameObject returnTile = null;
		returnTile = levelScript.GetTile(horizontal, vertical);
		return returnTile;		
	}
	
	void PaintNodeBlue(Node node){
		GameObject tile = FindTileByNode(node);
		levelScript.ChangeTileMaterial(tile, blue);
	}
	
	void PaintNodeRed(Node node){
		GameObject tile = FindTileByNode(node);
		levelScript.ChangeTileMaterial(tile, red);
	}
	
	void PaintOpenListNodes(){
		foreach (Node node in openList) {
			PaintNodeBlue (node);
		}
	}
	
	void PaintClosedListNodes(){
		foreach (Node node in closedList) {
			PaintNodeRed (node);
		}
	}
	
	
}
