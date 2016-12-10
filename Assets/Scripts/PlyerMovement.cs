using UnityEngine;
using System.Collections;

public class PlyerMovement : MonoBehaviour {
	public float speed = 6f;
	Vector3 movement;
	Rigidbody playerRigidbody;

	float rotationX = 0f;
	float rotationY = 0f;

	void Awake() {
		playerRigidbody = GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void FixedUpdate () {
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
	
		float deltaRotationX = Input.GetAxis ("Mouse X");
		float deltaRotationY = -Input.GetAxis ("Mouse Y");

		Rotate (deltaRotationX, deltaRotationY);
		Move (h, v);
	}


	void Move(float h, float v) {
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		movement = transform.TransformDirection (movement);
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Rotate(float deltaRotationX, float deltaRotationY) {
		rotationX += deltaRotationY; //  YES, IT IS SO! >_<
		rotationY += deltaRotationX;
		transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
	}
}
