using UnityEngine;
using System.Collections;

public class SpawnData {
	public GameObject spawnObject;
	public float spawnTimes;
	
	public SpawnData(GameObject sObject, float sTimes){
		spawnObject = sObject;
		spawnTimes = sTimes;
	}
}
