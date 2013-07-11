using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinderScript : MonoBehaviour {
	private LevelScript levelScript;
	private List<PathNode> closedList = new List<PathNode>();
	private List<PathNode> openList = new List<PathNode>();
	private List<PathNode> allNodes = new List<PathNode>();
	public List<PathNode> enemyPath = new List<PathNode>();
	public pathFinderAlgorithm alg = pathFinderAlgorithm.aStar;
	public Material blue;
	public Material red;
	public Material green;
	// Use this for initialization
	void Start () {
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelScript>();
		//PrintNodeList();	
		FindShortestPathBetweenGates();
	}
	
	public enum pathFinderAlgorithm {
		aStar = 1,
		breadthFirst = 2,
	};
	
	public void RegisterNode(PathNode node){
		//Add node to the list
		allNodes.Add(node);
	}
		
	public List<PathNode> GetPath(){
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
		if(alg == pathFinderAlgorithm.aStar)
			FindShortestPathAStar(start, end);
		else if(alg == pathFinderAlgorithm.breadthFirst)
			enemyPath = FindShortestPathBFF(start, end);
		else
			FindShortestPathAStar(start, end);
		//PaintList(closedList, red);
		//PaintList(openList, blue);
		PaintList(enemyPath, green);
	}
	
	public List<PathNode> FindShortestPathBFF(PathNode start, PathNode end){
		PathNode startNode, endNode, node;
		endNode = end;
		startNode = start;
		openList.Clear();
		ClearNodes();
		List<PathNode> neighbors = new List<PathNode>();
		openList.Add(startNode);
		startNode.opened = true;
		while(openList.Count != 0){
			node = openList[0];
			openList.Remove(node);
			closedList.Add (node);
			node.closed = true;
			if(node.nodePosition.x == endNode.nodePosition.x && node.nodePosition.y == endNode.nodePosition.y){
				return BacktracePath(node);
			}
			neighbors.Clear();
			FindNeighborTilesOnly(node, neighbors);
			foreach(PathNode neighborNode in neighbors){
				if(neighborNode.closed || neighborNode.opened){
					continue;
				}
				openList.Add(neighborNode);
				neighborNode.opened = true;
				neighborNode.parentNode = node;
			}
		}
		enemyPath.Clear();
		Debug.Log ("There is no available path from start to destination!");
		return enemyPath;
	}
	
	private void ClearNodes(){
		foreach(PathNode node in allNodes){
			node.closed = false;
			node.opened = false;
		}
	}
	
	private List<PathNode> BacktracePath(PathNode node){
		List<PathNode> result = new List<PathNode>();
		result.Add(node);
		while(node.parentNode != null){
			node = node.parentNode;
			result.Add(node);
		}
		return result;
	}
	
	void FindShortestPathAStar(PathNode start, PathNode end){		
		
		bool isInClosedList=false, isInOpenList=false;
		float tentative_GScore;
		PathNode activeNode;
		
		// Add target node to enemyPath
		enemyPath.Add (end);
		
		// Add start node to openList and calculate its costs
		openList.Add (start);
		openList[0].nodeG = 0;
		Calculate_H(end, openList[0]);
		openList[0].nodeF = openList[0].nodeG + openList[0].nodeH;
		
		while(openList.Count != 0) {
			// Find lowest cost node in openList 
			activeNode = FindLowestCost(openList); 
			
			// Check to see if the node is the target
			if (activeNode.nodePosition == end.nodePosition) {
				FindEnemyPath(closedList[0], activeNode);
				enemyPath.Add (start);
				break;
			} else {
				// Add activeNode to closedList and remove from openList
				closedList.Add (activeNode);
				openList.Remove (activeNode);
				
				// Find neighbor nodes of activeNode (east, west, north, south)
				List<PathNode> neighborNodesList = new List<PathNode>();
				FindNeighborNodes(activeNode, neighborNodesList);

				foreach (PathNode neighbor in neighborNodesList) {
					
					// Calculate tentative_GScore to move from activeNode to neighbor node
					tentative_GScore = activeNode.nodeG + (Mathf.Abs(activeNode.nodePosition.x-neighbor.nodePosition.x) + Mathf.Abs(activeNode.nodePosition.y-neighbor.nodePosition.y)) + neighbor.nodeHandicap;
					
					// Check to see if neighbor node is already in openList
					isInOpenList = isNeighborInOpenList(neighbor, openList);
					
					// Check to see if neighbor node is already in closedList
					isInClosedList = isNeighborInClosedList(neighbor, closedList);
					
					// if neighbor node is in closedList, continue..
	            	if (!isInClosedList) {
						
						if (!isInOpenList) {
							// set the neigbor nodes parent to activeNode, calculate its costs, and add to openList
							neighbor.parentNode = activeNode;
							neighbor.nodeG = tentative_GScore;
							Calculate_H(end, neighbor);
							neighbor.nodeF = neighbor.nodeG + neighbor.nodeH;
							openList.Add (neighbor);
						} else {
							// if the tentative_GScore is better, then change the parentNode and update its costs
							if (tentative_GScore < neighbor.nodeG) {
								neighbor.parentNode = activeNode;
								neighbor.nodeG = tentative_GScore;
								Calculate_H(end, neighbor);
								neighbor.nodeF = neighbor.nodeG + neighbor.nodeH;
							}
						}
					}
				}
			}
		}
	}
	
	void FindEnemyPath(PathNode start, PathNode end) {
		PathNode currentNode = end;
		
		do {
			enemyPath.Add(currentNode);
			currentNode = currentNode.parentNode;
		} while(currentNode != start);		
	}
	
	bool isNeighborInOpenList(PathNode neighbor, List<PathNode> openList) {
		
		foreach (PathNode node in openList) {
			if (node.nodePosition == neighbor.nodePosition)
				return true;
		}
		return false;
	}
	
	bool isNeighborInClosedList(PathNode neighbor, List<PathNode> closedList) {
		
		foreach (PathNode node in closedList) {
			if (node.nodePosition == neighbor.nodePosition)
				return true;
		}
		return false;
	}
	
	void FindNeighborTilesOnly(PathNode activeNode, List<PathNode> neighborNodes){
		foreach (PathNode node in allNodes) {
			if((activeNode.nodePosition.x+1 == node.nodePosition.x && activeNode.nodePosition.y == node.nodePosition.y) && FindTileByNode(node).tag == "Tile")
				neighborNodes.Add (node);
			
			if((activeNode.nodePosition.x-1 == node.nodePosition.x && activeNode.nodePosition.y == node.nodePosition.y) && FindTileByNode(node).tag == "Tile")
				neighborNodes.Add (node);
			
			if((activeNode.nodePosition.x == node.nodePosition.x && activeNode.nodePosition.y+1 == node.nodePosition.y) && FindTileByNode(node).tag == "Tile")
				neighborNodes.Add (node);
			
			if((activeNode.nodePosition.x == node.nodePosition.x && activeNode.nodePosition.y-1 == node.nodePosition.y) && FindTileByNode(node).tag == "Tile")
				neighborNodes.Add (node);
		}
	}
	
	void FindNeighborNodes(PathNode activeNode, List<PathNode> neighborNodes) {
		foreach (PathNode node in allNodes) {
			// Find east neighbor of activeNode in allNodes
			if((activeNode.nodePosition.x+1 == node.nodePosition.x && activeNode.nodePosition.y == node.nodePosition.y)&& FindTileByNode(node).tag != "Border")
				neighborNodes.Add (node);
			
			// Find west neighbor of activeNode in allNodes
			if((activeNode.nodePosition.x-1 == node.nodePosition.x && activeNode.nodePosition.y == node.nodePosition.y)&& FindTileByNode(node).tag != "Border")
				neighborNodes.Add (node);
			
			// Find north neighbor of activeNode in allNodes
			if((activeNode.nodePosition.x == node.nodePosition.x && activeNode.nodePosition.y+1 == node.nodePosition.y)&& FindTileByNode(node).tag != "Border")
				neighborNodes.Add (node);
			
			// Find south neighbor of activeNode in allNodes
			if((activeNode.nodePosition.x == node.nodePosition.x && activeNode.nodePosition.y-1 == node.nodePosition.y)&& FindTileByNode(node).tag != "Border")
				neighborNodes.Add (node);
		}
	}
	
	void Calculate_H(PathNode end, PathNode node) {
		// nodeH = the estimated(heuristic) cost to reach the destination from here.
		// node.nodeH = Mathf.Abs(end.nodePosition.x - node.nodePosition.x) + Mathf.Abs(end.nodePosition.y - node.nodePosition.y);
		node.nodeH = Mathf.Sqrt(Mathf.Pow((end.nodePosition.x - node.nodePosition.x), 2) + Mathf.Pow((end.nodePosition.y - node.nodePosition.y), 2));
		node.nodeH = node.nodeH + node.nodeHandicap;

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
	
	public GameObject FindTileByNode(PathNode node){
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
