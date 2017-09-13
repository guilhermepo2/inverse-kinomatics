using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullableScript : MonoBehaviour {

	public GameObject pullableObject;
	public GameObject rightHandIK;
	public GameObject leftHandIK;
	
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			other.GetComponent<SadGuyController>().isOnPullableArea(pullableObject, rightHandIK.transform, leftHandIK.transform, this.transform);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") {
			other.GetComponent<SadGuyController>().leftPullableArea();
		}
	}
}
