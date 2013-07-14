using UnityEngine;
using System.Collections;

public class FrogScript : EnemyBaseScript {
	// Use this for initialization
	protected override void Start () {
		base.Start();
		speed = 0.05f;
		hitPoints = 50f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();			
	}
}
