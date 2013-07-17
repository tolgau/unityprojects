using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {
	
	protected float curHealth;
	protected float maxHealth;

	protected Texture2D background;
	protected Texture2D foreground;

	// Use this for initialization
	void Start () {
		maxHealth = this.GetComponent<EnemyBaseScript>().hitPoints;
		
		background = new Texture2D(1, 1, TextureFormat.RGB24, false);
        foreground = new Texture2D(1, 1, TextureFormat.RGB24, false);
		
		background.SetPixel(0, 0, Color.black);
        foreground.SetPixel(0, 0, Color.red);
		
		background.Apply();
        foreground.Apply();
	}
	
	void OnGUI() {

		Vector3 screenPosition = Camera.current.WorldToScreenPoint(transform.position); // gets screen position.
		screenPosition.y = Screen.height - (screenPosition.y + 1); // inverts y

		GUI.DrawTexture(new Rect(screenPosition.x-19/2, screenPosition.y-8, 19, 4),background);
		GUI.DrawTexture(new Rect(screenPosition.x-19/2, screenPosition.y-8, curHealth/maxHealth*19, 4),foreground);
	}
	
	// Update is called once per frame
	void Update () {
		curHealth = this.GetComponent<EnemyBaseScript>().hitPoints;
	}
}
