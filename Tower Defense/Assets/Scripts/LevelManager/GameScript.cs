using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {
	public GameObject levelScript;
	public GameObject saveLoad;
	public GameObject mainCamera;
	
	// Use this for initialization
	void Start () {
			Instantiate(mainCamera);
	}
	
	void OnGUI () {
        GUILayout.BeginArea(new Rect((Screen.width-100)/2,(Screen.height-50)/2,100,50));
		if(GUILayout.Button("Play")){
			Instantiate(levelScript);
			Instantiate(saveLoad);
			enabled = false;
		}
		
		if(GUILayout.Button("Quit"))
			Application.Quit();
		GUILayout.EndArea();		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
