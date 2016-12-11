using UnityEngine;
using System.Collections;

public class PlyerMovement : MonoBehaviour {
	public float speed = 6f;
	Vector3 movement;
	Rigidbody playerRigidbody;
	CharacterController characterController;

	bool needLockScreen = true;
	public float sense = 30f;
	public float gravity = 1f;
	public float mouseSensitivity = 5.0f;
	public float upDownRange = 60.0f;

	float verticalRotation = 0.0f;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		//characterController.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("escape")) {
			print ("escape key was pressed");
			needLockScreen = false;
		}
		if(needLockScreen)
			Screen.lockCursor = true;
		else
			Screen.lockCursor = false;

		float h = Input.GetAxisRaw ("Horizontal") * sense;
		float v = Input.GetAxisRaw("Vertical") * sense;

		float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
		transform.Rotate (0, rotLeftRight, 0);

		verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);

		Move (h, v);
	}

	void Move(float h, float v) {
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		movement = transform.TransformDirection (movement);
		if(characterController.isGrounded)
			print("grounded");
		else
			print("not grounded");
		//playerRigidbody.MovePosition (transform.position + movement);
		characterController.Move(movement);		
		characterController.Move(Vector3.down * gravity * Time.deltaTime);		
		//playerRigidbody.AddForce (movement, ForceMode.Impulse);
	}
}
