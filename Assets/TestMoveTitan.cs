﻿using UnityEngine;

public class TestMoveTitan : MonoBehaviour {

	[SerializeField] float force = 40000f;
	[SerializeField] KeyCode left = KeyCode.Q;
	[SerializeField] KeyCode right = KeyCode.E;
	public Rigidbody2D titanTotalRB;

	void Start () {
		titanTotalRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		// hInput = Input.GetAxis("Horizontal");
		float input;
		if (Input.GetKey(right))
			input = 1f;
		else if (Input.GetKey(left))
			input = -1f;
		else input = 0;

		//Move the titan
		titanTotalRB.AddForce(Vector2.right * input * force);
		// transform.Translate(Vector3.right * hInput * speed);
		// transform.position.x += hInput * speed;
		
	}
}
