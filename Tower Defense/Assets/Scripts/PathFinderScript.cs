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
		
	public List<PathNode> GetPath(){
		//Debug.Log(enemyPath.Count);
		return enemyPath;
	}
	
	public PathNode GetNode(float mapHor, float mapVer){
		PathNode result=null;
		foreach (PathNode node in allNodes)
		{
		    if(node.nodePosition.x == mapHor && node.nodePosition.y == mapVer)
				result = node;
		}
		return result;
	}
	
	public void RegisterAsNode(GameObject gameObject){
		PathNode node = new PathNode();
		node.nodePosition = gameObject.transform.position;
		if (gameObject.tag != "Tile"){
			node.nodeHandicap = 1000;
		} else {
			node.nodeHandicap = 0;
		}
		RegisterNode(node);
	}
	
	public void FindShortestPathBetweenGates(){
		GameObject[] gateExit = GameObject.FindGameObjectsWithTag("GateExit");
		GameObject[] gateEnter = GameObject.FindGameObjectsWithTag("GateEnter");
		PathNode start = new PathNode();
		PathNode end = new PathNode();
		start.nodePosition = gateEnter[0].transform.position;
		start.nodePosition.y--;
		start.nodePosition.z = 0f;
		end.nodePosition = gateExit[0].transform.position;
		end.nodePosition.y++;
		end.nodePosition.z = 0f;
		InitiatePathFinder();
		FindShortestPathBetweenNodes(start, end);
		PaintList(closedList, red);
		PaintList(openList, blue);
		PaintList(enemyPath, green);
	}
	
	void FindShortestPathBetweenNodes(PathNode start, PathNode end){		
		bool isInClosedList=false, isInOpenList=false;
		float tentative_GScore;
		PathNode nodeWithLowestCost;
		
		openList.Add (start);
		enemyPath.Add (end);
		Calculate_G(start, openList[0]);
		Calculate_H(end, openList[0]);
		openList[0].nodeF = openList[0].nodeG + openList[0].nodeH;
		
		while(openList.Count != 0) {//do {
			nodeWithLowestCost = FindLowestCost(openList); 
						
			if (nodeWithLowestCost.nodePosition == end.nodePosition) {
				FindEnemyPath(closedList[0], closedList[closedList.Count-1]);
				enemyPath.Add (start);
				break;
			} else {				
				closedList.Add (nodeWithLowestCost);
				openList.Remove (nodeWithLowestCost);

				List<PathNode> neighborNodesList = new List<PathNode>();
				FindNeighborNodes(nodeWithLowestCost, neighborNodesList);

				foreach (PathNode neighbor in neighborNodesList) {
					
					Calculate_G(start, neighbor);
					Calculate_H(end, neighbor);
					neighbor.nodeF = neighbor.nodeG + neighbor.nodeH;
					
					isInClosedList = false;
					isInOpenList = false;
					tentative_GScore = nodeWithLowestCost.nodeG + (Mathf.Abs(nodeWithLowestCost.nodePosition.x-neighbor.nodePosition.x) + Mathf.Abs(nodeWithLowestCost.nodePosition.y-neighbor.nodePosition.y)) + neighbor.nodeHandicap;
					
					foreach (PathNode node in openList) {
						if (node.nodePosition == neighbor.nodePosition) {
							isInOpenList |= true;
						} else {
							isInOpenList |= false;
						}
					}
				
					foreach (PathNode node in closedList) {
						if (node.nodePosition == neighbor.nodePosition) {
							isInClosedList |= true;
						} else {
							isInClosedList |= false;
						}
					}

	            	if (isInClosedList && (tentative_GScore >= neighbor.nodeG)) {
						continue;
					}
					
					if (!isInOpenList || (tentative_GScore < neighbor.nodeG)) {
						neighbor.parentNode = nodeWithLowestCost;
						neighbor.nodeG = tentative_GScore;
						Calculate_H(end, neighbor);
						neighbor.nodeF = neighbor.nodeG + neighbor.nodeH;

						if (!isInOpenList) {
							openList.Add (neighbor);
						} 
					}
				}
				
			}
		} //while(closedList[closedList.Count - 1].nodePosition != end.nodePosition);
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
	
	void Calculate_G(PathNode start, PathNode node) {
		//node.nodeG = Mathf.Sqrt(Mathf.Pow((start.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((start.nodePosition.y - node.nodePosition.y), 2));
		node.nodeG = Mathf.Abs(start.nodePosition.x - node.nodePosition.x) + Mathf.Abs(start.nodePosition.y - node.nodePosition.y);
		node.nodeG = node.nodeG + node.nodeHandicap;
		//node.nodeH = Mathf.Sqrt(Mathf.Pow((end.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((end.nodePosition.y - node.nodePosition.y), 2));
		//node.nodeF = node.nodeH + node.nodeG + node.nodeHandicap;
	}
	
	void Calculate_H(PathNode end, PathNode node) {
		// nodeG = the exact cost to reach this node from the starting node.
		// nodeH = the estimated(heuristic) cost to reach the destination from here.
		// nodeF = nodeG + nodeH  As the algorithm runs the F value of a node tells us how expensive we think it will be to reach our goal by way of that node.
		//node.nodeG = Mathf.Sqrt(Mathf.Pow((start.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((start.nodePosition.y - node.nodePosition.y), 2));
		//node.nodeG = Mathf.Abs(start.nodePosition.x - node.nodePosition.x) + Mathf.Abs(start.nodePosition.y - node.nodePosition.y);
		node.nodeH = Mathf.Sqrt(Mathf.Pow((end.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((end.nodePosition.y - node.nodePosition.y), 2));
		node.nodeH = node.nodeH + node.nodeHandicap;
		//node.nodeF = node.nodeG + node.nodeH;
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
	
	void PaintNode(PathNode node, Material color){
		GameObject tile = FindTileByNode(node);
		levelScript.ChangeTileMaterial(tile, color);
	}
	
	void InitiatePathFinder(){
		ResetListTilesToPrefab();
		closedList.Clear();
		openList.Clear();
		enemyPath.Clear();
	}
	
	void ResetListTilesToPrefab(){
		foreach (PathNode node in openList) {
			GameObject tile = FindTileByNode(node);
			TileScript tempTileScript = tile.GetComponent<TileScript>();
			tempTileScript.RevertMaterialToPrefab();
		}
		foreach (PathNode node in closedList) {
			GameObject tile = FindTileByNode(node);
			TileScript tempTileScript = tile.GetComponent<TileScript>();
			tempTileScript.RevertMaterialToPrefab();
		}
		foreach (PathNode node in enemyPath) {
			GameObject tile = FindTileByNode(node);
			TileScript tempTileScript = tile.GetComponent<TileScript>();
			tempTileScript.RevertMaterialToPrefab();
		}
		
	}
	
	void PaintList(List<PathNode> list, Material color){
		foreach (PathNode node in list) {
			PaintNode (node, color);
		}
	}	
}
