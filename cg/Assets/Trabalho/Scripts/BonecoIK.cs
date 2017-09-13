using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonecoIK : MonoBehaviour {

	public GameObject target;
	private Animator anim;
	private float weight = 1f;

	void OnAnimatorIK(int layerIndex) {
		anim.SetIKPosition(AvatarIKGoal.RightHand, target.transform.position);
		anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
	}
	
	void Start () {
		anim = this.GetComponent<Animator>();
	}
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.X)) {
			target = null;
		}

		if(Input.GetKeyDown(KeyCode.Y)) {
			target = GameObject.Find("Sphere");
		}	
	}
}
