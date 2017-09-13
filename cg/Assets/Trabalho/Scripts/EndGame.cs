using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {


	public Texture2D fadeOutTexture;
	private float fadeSpeed = 0.1f;
	private int drawDepth = -1000;
	private float alpha = 0.0f;
	private int fadeDir = 1;
	private bool fade = false;

	void OnGUI() {
		if(fade) {
			alpha += fadeDir * fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			GUI.depth = drawDepth;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
		}
	}

	public float BeginFade(int direction) {
		fadeDir = direction;
		return (fadeSpeed);
	}
	
	private IEnumerator fadeOutForever() {
		float fadeTime = BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("collision!");
		fade = true;
		StartCoroutine(fadeOutForever());
	}
}
