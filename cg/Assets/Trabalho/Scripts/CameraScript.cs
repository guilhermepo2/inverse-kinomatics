using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public GameObject playerReference;

	public Material processingImageMaterial;
	void OnRenderImage(RenderTexture imageFromRenderedImage, RenderTexture imageDisplayedOnScreen) {
		if(processingImageMaterial != null) {
			Graphics.Blit(imageFromRenderedImage, imageDisplayedOnScreen, processingImageMaterial);
		}
	}

	void Update() {
		this.transform.position = new Vector3(
			playerReference.transform.position.x + 4f,
			this.transform.position.y,
			this.transform.position.z
		);
	}
}
