using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatActivation : MonoBehaviour {

	public GameObject boat;
	public GameObject rightIK;
	public GameObject lookIK;

	private bool activated;
	private Transform playerTransform;
	private float angle = 0;

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			other.GetComponent<SadGuyController>().isOnBoat(rightIK.transform, lookIK.transform);
			//other.GetComponent<Rigidbody>().isKinematic = true;
			//other.GetComponent<CapsuleCollider>().enabled = false;
			//other.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
			playerTransform = other.gameObject.transform;
			activated = true;
		}
	}

	void Update() {
		if(activated) {
			angle += 0.1f;
			boat.transform.Translate(0.0f, Mathf.Cos(angle) * Time.deltaTime, 1.0f * Time.deltaTime);
			Vector3 playerRotation = playerTransform.localEulerAngles;
			playerRotation.y = 90f;
			playerTransform.localEulerAngles = playerRotation;
			playerTransform.Translate(0.0f, Mathf.Cos(angle) * Time.deltaTime, 1.0f * Time.deltaTime);
		}
	}
}
