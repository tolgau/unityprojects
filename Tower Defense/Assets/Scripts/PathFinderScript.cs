using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinderScript : MonoBehaviour {
	private LevelScript levelScript;
	private List<PathNode> closedList = new List<PathNode>();
	private List<PathNode> openList = new List<PathNode>();
	private List<PathNode> allNodes = new List<PathNode>();
	public List<PathNode> enemyPath = new List<PathNode>();
	public Material blue;
	public Material red;
	public Material green;
	// Use this for initialization
	void Start () {
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		//PrintNodeList();	
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
	
	public void RegisterAsNode(GameObject gameObject){
		PathNode node = new PathNode();
		node.nodePosition = gameObject.transform.position;
		node.associatedObject = gameObject;
		RegisterNode(node);
		//TODO Switch statement which looks at an object's tag or attribute and decides for a handicap value to be used in F calculation.
		//return node;
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
		//PaintOpenListNodes();
		//PaintClosedListNodes();
		PaintPathList();
	}
	
	void FindShortestPathBetween(PathNode start, PathNode end){		
		
		bool isClosedList=false, isOpenList=false;
		float tentative_GScore;
		PathNode nodeWithLowestCost;
			
		openList.Add (start);
		enemyPath.Add (start);
		CalculateCost(start, end, openList);
		
		do {
			nodeWithLowestCost = FindLowestCost(openList); 
						
			if (nodeWithLowestCost.nodePosition == end.nodePosition) {
				//Debug.Log (closedList[0].nodePosition + " " + closedList[closedList.Count-1].nodePosition);
				FindEnemyPath(closedList[0], closedList[closedList.Count-1]);
				enemyPath.Add (end);
				break;
			} else {				
				closedList.Add (nodeWithLowestCost);
				openList.Remove (nodeWithLowestCost);
			
				List<PathNode> neighborNodesList = new List<PathNode>();
				FindNeighborNodes(nodeWithLowestCost, neighborNodesList);
				CalculateCost(start, end, neighborNodesList);

				foreach (PathNode neighbor in neighborNodesList) {
					
					isClosedList = false;
					isOpenList = false;
					tentative_GScore = nodeWithLowestCost.nodeG + (Mathf.Abs(nodeWithLowestCost.nodePosition.x-neighbor.nodePosition.x) + Mathf.Abs(nodeWithLowestCost.nodePosition.y-neighbor.nodePosition.y));
										
					foreach (PathNode node in openList) {
						if (node.nodePosition == neighbor.nodePosition) {
							isOpenList |= true;
						} else {
							isOpenList |= false;
						}
					}
				
					foreach (PathNode node in closedList) {
						if (node.nodePosition == neighbor.nodePosition) {
							isClosedList |= true;
						} else {
							isClosedList |= false;
						}
					}

	            	if (isClosedList && (tentative_GScore >= neighbor.nodeG)) {
						continue;
					}
					
					if (!isOpenList || (tentative_GScore < neighbor.nodeG)) {
						neighbor.parentNode = nodeWithLowestCost;

						if (!isOpenList) {
							openList.Add (neighbor);
							CalculateCost(start, end, openList);
						}
					}
				}
				
			}
		} while(closedList[closedList.Count - 1].nodePosition != end.nodePosition);
	}
	
	void FindEnemyPath(PathNode start, PathNode end) {
		PathNode currentNode = end;
		//Debug.Log(currentNode.parentNode);
		
		while(currentNode != start) {
			enemyPath.Add(currentNode);
			currentNode = currentNode.parentNode;
		}		
	}
	
	void FindNeighborNodes(PathNode activeNode, List<PathNode> neighborNodes) {
		foreach (PathNode node in allNodes) {
			if(activeNode.nodePosition.x+1 == node.nodePosition.x && activeNode.nodePosition.y == node.nodePosition.y)
				neighborNodes.Add (node);
			
			if(activeNode.nodePosition.x-1 == node.nodePosition.x && activeNode.nodePosition.y == node.nodePosition.y)
				neighborNodes.Add (node);
			
			if(activeNode.nodePosition.x == node.nodePosition.x && activeNode.nodePosition.y+1 == node.nodePosition.y)
				neighborNodes.Add (node);
			
			if(activeNode.nodePosition.x == node.nodePosition.x && activeNode.nodePosition.y-1 == node.nodePosition.y)
				neighborNodes.Add (node);
		}
	}
	
	void CalculateCost(PathNode start, PathNode end, List<PathNode> list) {
		// nodeG = the exact cost to reach this node from the starting node.
		// nodeH = the estimated(heuristic) cost to reach the destination from here.
		// nodeF = nodeG + nodeH  As the algorithm runs the F value of a node tells us how expensive we think it will be to reach our goal by way of that node.
		foreach (PathNode node in list) {
			node.nodeG = Mathf.Sqrt(Mathf.Pow((start.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((start.nodePosition.y - node.nodePosition.y), 2));
			node.nodeH = Mathf.Sqrt(Mathf.Pow((end.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((end.nodePosition.y - node.nodePosition.y), 2));
			node.nodeF = node.nodeH + node.nodeG;
		}
	}
	
	PathNode FindLowestCost(List<PathNode> openList) {
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
	
	void PaintNodeGreen(PathNode node){
		GameObject tile = FindTileByNode(node);
		levelScript.ChangeTileMaterial(tile, green);
	}

	
	void PaintOpenListNodes(){
		foreach (PathNode node in openList) {
			PaintNodeBlue (node);
		}
	}
	
	void PaintPathList(){
		foreach (PathNode node in enemyPath) {
			PaintNodeGreen (node);
		}
	}
		
	void PaintClosedListNodes(){
		foreach (PathNode node in closedList) {
			PaintNodeRed (node);
		}
	}
	
	
}
