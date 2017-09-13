using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float forwardForce = 500.0f;
	private float sideForce = 500.0f; 
	private float jumpForce = 300.0f;
	private Rigidbody rb;

	void Start () {
		rb = this.GetComponent<Rigidbody>();	
	}
	
	void FixedUpdate () {

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		
		rb.AddForce(-horizontal * Time.deltaTime * forwardForce,
		0.0f,
		-vertical * Time.deltaTime * sideForce);

		if(Input.GetKeyDown(KeyCode.Space)) {
			rb.AddForce(0.0f, jumpForce, 0.0f);
		}
	}
}
