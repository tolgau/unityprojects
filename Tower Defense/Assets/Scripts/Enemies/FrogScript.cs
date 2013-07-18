using UnityEngine;
using System.Collections;

public class FrogScript : EnemyBaseScript {
	// Use this for initialization
	
	protected override void Awake ()
	{
		base.Awake ();
		speed = 0.05f;
		hitPoints = 50f;
	}
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();			
	}
}
