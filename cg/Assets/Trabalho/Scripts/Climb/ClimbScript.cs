using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbScript : MonoBehaviour {

	public GameObject rootPosition;
	public GameObject target;

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			other.GetComponent<SadGuyController>().setClimb(rootPosition, target.transform);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") {
			other.GetComponent<SadGuyController>().unsetClimb();
		}
	}


}
