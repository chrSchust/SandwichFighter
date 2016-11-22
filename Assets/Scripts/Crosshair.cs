using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	public float crosshairSizeMultiplicator = 0.1f;
	public Texture crosshairTexture;
	Rect crosshairRect;
	// Use this for initialization
	void Start () {
		float crosshairSize = Screen.width * crosshairSizeMultiplicator;
		//crosshairTexture = Resources.Load ("Textures/crosshair") as Texture;
		crosshairRect = new Rect (Screen.width / 2 - crosshairSize / 2, 
			Screen.height / 2 - crosshairSize / 2,
			crosshairSize,
			crosshairSize);
	}

	void OnGUI() {
		GUI.DrawTexture (crosshairRect, crosshairTexture);
	}
}
