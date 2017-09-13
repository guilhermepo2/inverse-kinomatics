using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableActivation : MonoBehaviour {

	public GameObject pushableObject;
	public GameObject rightHandIK;
	public GameObject leftHandIK;
	
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			other.GetComponent<SadGuyController>().isOnPushableArea(pushableObject, rightHandIK.transform, leftHandIK.transform, this.transform);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") {
			other.GetComponent<SadGuyController>().leftPushableArea();
		}
	}
}
