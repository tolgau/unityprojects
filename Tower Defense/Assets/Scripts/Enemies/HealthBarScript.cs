using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {
	
	protected float curHealth;
	protected float maxHealth;

	protected Texture2D background;
	protected Texture2D foreground;
	protected Vector3 screenPosition;

	// Use this for initialization
	void Start () {
		curHealth = this.GetComponent<EnemyBaseScript>().hitPoints;
		maxHealth = this.GetComponent<EnemyBaseScript>().hitPoints;
		
		background = new Texture2D(1, 1, TextureFormat.RGB24, false);
        foreground = new Texture2D(1, 1, TextureFormat.RGB24, false);
		
		background.SetPixel(0, 0, Color.black);
        foreground.SetPixel(0, 0, Color.red);
		
		background.Apply();
        foreground.Apply();
	}
	
	void OnGUI() {
		if(maxHealth != 0){
			GUI.DrawTexture(new Rect(screenPosition.x-11, screenPosition.y-16, 25, 4),background);
			GUI.DrawTexture(new Rect(screenPosition.x-11, screenPosition.y-16, curHealth/maxHealth*25, 4),foreground);
		}
	}

	// Update is called once per frame
	void Update () {
		screenPosition = Camera.main.WorldToScreenPoint(transform.position); // gets screen position.
		screenPosition.y = Screen.height - (screenPosition.y + 1); // inverts y
		curHealth = this.GetComponent<EnemyBaseScript>().hitPoints;
	}
}
