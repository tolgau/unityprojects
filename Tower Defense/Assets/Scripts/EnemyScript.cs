using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(-Input.GetAxis("Horizontal")*Time.deltaTime*10,0,0));
		transform.Translate(new Vector3(0,-Input.GetAxis("Vertical")*Time.deltaTime*10,0));
	}
}
