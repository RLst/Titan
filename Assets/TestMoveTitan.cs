using UnityEngine;

public class TestMoveTitan : MonoBehaviour {

	public float speed = 1f;

	float hInput, vInput;

	public Rigidbody2D titanTotalRB;

	void Start () {
		titanTotalRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		hInput = Input.GetAxis("Horizontal");

		//Move the titan
		titanTotalRB.AddForce(Vector2.right * hInput * speed);
		// transform.Translate(Vector3.right * hInput * speed);
		// transform.position.x += hInput * speed;
		
	}
}
