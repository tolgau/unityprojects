using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {
	public GameObject levelScript;
	public GameObject saveLoad;
	
	// Use this for initialization
	void Start () {
		Instantiate(levelScript);
		Instantiate(saveLoad);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
