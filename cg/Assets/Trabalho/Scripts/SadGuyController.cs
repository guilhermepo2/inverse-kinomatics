using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadGuyController : MonoBehaviour {

	private Animator anim;
	RigidbodyConstraints normalConstraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
	
	
	// climbing
	private bool canClimb = false;
	private GameObject rootClimb;
	private Transform climbTargetPosition;

	// pulling
	RigidbodyConstraints pullingConstraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
	private bool isPulling;
	private GameObject pullingInteractable;
	private Transform pullRoot;

	// pushing
	private bool isPushing;
	private GameObject pushingInteractable;
	private Transform pushRoot;

	// boat
	private bool onBoat = false;

	// IK 
	private bool pushIK = false;
	private bool pullIK = false;
	private bool lookIK = false;
	private float weight = 0.0f;
	private float lookWeight = 0.0f;
	private Transform rightHandIK;
	private Transform leftHandIK;
	public Transform lookIKT;

	
	// on boat
	public void isOnBoat(Transform right, Transform look) {
		weight = 1.0f;
		lookWeight = 1.0f;
		rightHandIK = right;
		lookIKT = look;
		onBoat = true;
		lookIK = true;
	}

	public void isOnPushableArea(GameObject interactingF, Transform rightHand, Transform leftHand, Transform root) {
		pushIK = true;
		pushingInteractable = interactingF;
		rightHandIK = rightHand;
		leftHandIK = leftHand;
		weight = 0.2f;
		pushRoot = root;
	}

	public void isOnPullableArea(GameObject interacting, Transform rightHand, Transform leftHand, Transform root) {
		pullIK = true;
		pullingInteractable = interacting;
		rightHandIK = rightHand;
		leftHandIK = leftHand;
		weight = 0.2f;
		pullRoot = root;
	}

	public void leftPushableArea() {
		pushIK = false;
		pushingInteractable = null;
		rightHandIK = null;
		leftHandIK = null;
		weight = 0.0f;
	}

	public void leftPullableArea() {
		pullIK = false;
		pullingInteractable = null;
		rightHandIK = null;
		leftHandIK = null;
		weight = 0.0f;
	}

	public void setClimb(GameObject root, Transform targetPosition) {
		rootClimb = root;
		climbTargetPosition = targetPosition;
		canClimb = true;
	}

	public void finishClimb() {
		this.transform.position = climbTargetPosition.position;
		climbTargetPosition = null;
	}

	public void unsetClimb() {
		rootClimb = null;
		canClimb = false;
	}

	public void runningClimb() {
		if(pullingInteractable != null) {
			this.transform.position = new Vector3(pullingInteractable.transform.position.x,
			0.0f,
			pullingInteractable.transform.position.z);
		} else if(pushingInteractable != null) {
			this.transform.position = new Vector3(pushingInteractable.transform.position.x,
			0.0f,
			pushingInteractable.transform.position.z);
		}
	}

	void Start () {
		anim = this.GetComponent<Animator>();
		isPushing = false;
		isPulling = false;
	}

	void OnAnimatorIK(int layerIndex) {
		if(pushIK || pullIK) {
			anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandIK.position);
			anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIK.position);
			anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
			anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
		}

		if(onBoat) {
			anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandIK.position);
			anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
		}

		if(lookIK) {
			anim.SetLookAtWeight(lookWeight);
			anim.SetLookAtPosition(lookIKT.position);
		}
	}
	
	void Update () {

		float horizontal = Input.GetAxis("Horizontal");


		if(horizontal < 0 && !isPulling && !isPushing) {
			this.transform.localEulerAngles = new Vector3(
				this.transform.localEulerAngles.x,
				-90f,
				this.transform.localEulerAngles.z
			);
		} else if (horizontal > 0 && !isPulling && !isPushing) {
			this.transform.localEulerAngles = new Vector3(
				this.transform.localEulerAngles.x,
				90f,
				this.transform.localEulerAngles.z
			);
		}

		if(Mathf.Abs(horizontal) > 0.5 && Input.GetKey(KeyCode.LeftCommand)) {
			anim.SetFloat("Blend", 0.49f);
		} else {
			anim.SetFloat("Blend", Mathf.Abs(horizontal));
		}


		// CODE FOR PULLING
		if(Input.GetKeyDown(KeyCode.E) && !isPulling && (pullingInteractable != null)) {
			this.GetComponent<Rigidbody>().constraints = pullingConstraints;
			anim.SetTrigger("beginPull");
			weight = 1.0f;
			isPulling = true;
			this.transform.position = pullRoot.position;
			pullingInteractable.transform.parent = this.transform;
			pullingInteractable.GetComponent<Rigidbody>().isKinematic = true;
		}

		if(Input.GetKeyUp(KeyCode.E) && isPulling) {
			anim.SetTrigger("finishPull");
			weight = 0.0f;
			pullIK = false;
			isPulling = false;
			//pullingInteractable.GetComponent<Rigidbody>().mass = 100;
			pullingInteractable.GetComponent<Rigidbody>().isKinematic = false;
			pullingInteractable.transform.parent = null;	
			this.GetComponent<Rigidbody>().constraints = normalConstraints;		
		}
		
		// CODE FOR PUSHING
		if(Input.GetKeyDown(KeyCode.E) && !isPushing && (pushingInteractable != null)) {
			anim.SetTrigger("beginPush");
			weight = 1.0f;
			isPushing = true;
			this.transform.position = pushRoot.position;
		}

		if(Input.GetKeyUp(KeyCode.E) && isPushing) {
			anim.SetTrigger("finishPush");
			pushingInteractable.GetComponent<Rigidbody>().mass = 100;
			weight = 0.0f;
			pushIK = false;
			isPushing = false;
		}

		if(isPushing && (pushingInteractable != null)) {
			pushingInteractable.GetComponent<Rigidbody>().mass = 0;
		}

		// JUMPING
		if(Input.GetKeyDown(KeyCode.Space) && !isPushing && !isPulling) {
			if(pushingInteractable != null || pullingInteractable != null) {
				anim.SetTrigger("climb");
			} else if(canClimb) {
				this.transform.position = rootClimb.transform.position;
				anim.SetTrigger("climbUp");
			} else {
				anim.SetTrigger("jump");
			}
		}

		anim.SetFloat("Blend", Mathf.Abs(horizontal));

	}
}
