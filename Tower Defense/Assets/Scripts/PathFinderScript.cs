using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinderScript : MonoBehaviour {
	private LevelScript levelScript;
	private List<PathNode> closedList = new List<PathNode>();
	private List<PathNode> openList = new List<PathNode>();
	private List<PathNode> allNodes = new List<PathNode>();
	public Material blue;
	public Material red;
	// Use this for initialization
	void Start () {
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();	
		FindShortestPathBetweenGates();
	}
	
	public void RegisterNode(PathNode node){
		//Add node to the list
		allNodes.Add(node);
	}
	
	public void PrintNodeList(){
		foreach(PathNode node in allNodes){
			Debug.Log(node.nodePosition);
		}
	}
	
	public PathNode RegisterAsNode(GameObject gameObject){
		PathNode node = new PathNode();
		node.nodePosition = gameObject.transform.position;
		node.associatedObject = gameObject;
		//TODO Switch statement which looks at an object's tag or attribute and decides for a handicap value to be used in F calculation.
		return node;
	}
	
	public void FindShortestPathBetweenGates(){
		GameObject[] gateExit = GameObject.FindGameObjectsWithTag("GateExit");
		GameObject[] gateEnter = GameObject.FindGameObjectsWithTag("GateEnter");
		PathNode start = new PathNode();
		PathNode end = new PathNode();
		start.nodePosition = gateEnter[0].transform.position;
		start.nodePosition.y--;
		end.nodePosition = gateExit[0].transform.position;
		end.nodePosition.y++;
		FindShortestPathBetween(start, end);
		PaintOpenListNodes();
		PaintClosedListNodes();
	}
	
	void FindShortestPathBetween(PathNode start, PathNode end){		
		bool isClosedList=false, isOpenList=false;
		openList.Add (start);
		//closedList.Add (start);
		CalculateF(start, end, openList);
		
		//while(closedList[closedList.Count - 1].nodePosition != end.nodePosition) {
			
			PathNode nodeWithLowestFValue = FindLowestFValue(openList); 
			if (nodeWithLowestFValue == end) {
				// this node is the goal then we're done
			} else {
				closedList.Add (nodeWithLowestFValue);
				openList.Remove (nodeWithLowestFValue);
			
				List<PathNode> neighborNodesList = new List<PathNode>();
				PathNode neighborNode1 = new PathNode();
				neighborNode1.nodePosition = nodeWithLowestFValue.nodePosition;
				
				neighborNode1.nodePosition.x = nodeWithLowestFValue.nodePosition.x+1;
				neighborNodesList.Add (neighborNode1);
			
				PathNode neighborNode2 = new PathNode();
				neighborNode2.nodePosition = nodeWithLowestFValue.nodePosition;
				neighborNode2.nodePosition.x = nodeWithLowestFValue.nodePosition.x-1;
				neighborNodesList.Add (neighborNode2);
			
				PathNode neighborNode3 = new PathNode();
				neighborNode3.nodePosition = nodeWithLowestFValue.nodePosition;
				neighborNode3.nodePosition.y = nodeWithLowestFValue.nodePosition.y+1;
				neighborNodesList.Add (neighborNode3);
			
				PathNode neighborNode4 = new PathNode();
				neighborNode4.nodePosition = nodeWithLowestFValue.nodePosition;
				neighborNode4.nodePosition.y = nodeWithLowestFValue.nodePosition.y-1;
				neighborNodesList.Add (neighborNode4);				

				foreach (PathNode neighbor in neighborNodesList) {
					foreach (PathNode node in openList) {
						if (node.nodePosition == neighbor.nodePosition) {
							isOpenList = true;
						} else {
							isOpenList = false;
						}
					}
				
					foreach (PathNode node in closedList) {
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
	
	void CalculateF(PathNode start, PathNode end, List<PathNode> openList) {
		
		foreach (PathNode node in openList) {
			node.nodeG = Mathf.Sqrt(Mathf.Pow((start.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((start.nodePosition.y - node.nodePosition.y), 2));
			node.nodeH = Mathf.Sqrt(Mathf.Pow((start.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((start.nodePosition.y - node.nodePosition.y), 2));
			node.nodeF = node.nodeH + node.nodeG;
		}
	}
	
	PathNode FindLowestFValue(List<PathNode> openList) {
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
	
	GameObject FindTileByNode(PathNode node){
		float horizontal = node.nodePosition.x;
		float vertical = node.nodePosition.y;
		GameObject returnTile = null;
		returnTile = levelScript.GetTile(horizontal, vertical);
		return returnTile;		
	}
	
	void PaintNodeBlue(PathNode node){
		GameObject tile = FindTileByNode(node);
		levelScript.ChangeTileMaterial(tile, blue);
	}
	
	void PaintNodeRed(PathNode node){
		GameObject tile = FindTileByNode(node);
		levelScript.ChangeTileMaterial(tile, red);
	}
	
	void PaintOpenListNodes(){
		foreach (PathNode node in openList) {
			PaintNodeBlue (node);
		}
	}
	
	void PaintClosedListNodes(){
		foreach (PathNode node in closedList) {
			PaintNodeRed (node);
		}
	}
	
	
}
