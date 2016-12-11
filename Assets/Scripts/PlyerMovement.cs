using UnityEngine;
using System.Collections;

public class PlyerMovement : MonoBehaviour {
	// public float speed = 6f;
	// Vector3 movement;
	// Rigidbody playerRigidbody;
	// CharacterController characterController;

	// float rotationX = 0f;
	// float rotationY = 0f;
	// bool needLockScreen = true;
	// public float sense = 30f;

	// void Awake() {
	// 	//playerRigidbody = GetComponent<Rigidbody> ();
	// 	characterController = GetComponent<CharacterController>();
	// 	characterController.enabled = true;
	// }

	// // Use this for initialization
	// void FixedUpdate () {
	// 	if (Input.GetKeyDown ("escape")) {
 //        	print ("escape key was pressed");
 //        	needLockScreen = false;
 //    	}
 //    	if(needLockScreen)
	// 		Screen.lockCursor = true;
	// 	else
	// 		Screen.lockCursor = false;

	// 	float h = Input.GetAxisRaw ("Horizontal") * sense;
	// 	float v = Input.GetAxisRaw("Vertical") * sense;
	
	// 	float deltaRotationX = Input.GetAxis ("Mouse X");
	// 	float deltaRotationY = -Input.GetAxis ("Mouse Y");

	// 	Rotate (deltaRotationX, deltaRotationY);
	// 	Move (h, v);
	// }


	// void Move(float h, float v) {
	// 	movement.Set (h, 0f, v);
	// 	movement = movement.normalized * speed * Time.deltaTime;
	// 	movement = transform.TransformDirection (movement);
	// 	if(characterController.isGrounded)
	// 		print("grounded");
	// 	else
	// 		print("not grounded");
	// 	//playerRigidbody.MovePosition (transform.position + movement);
	// 	characterController.Move(movement);		
	// 	//playerRigidbody.AddForce (movement, ForceMode.Impulse);
	// }

	// void Rotate(float deltaRotationX, float deltaRotationY) {
	// 	rotationX += deltaRotationY; //  YES, IT IS SO! >_<
	// 	rotationY += deltaRotationX;
	// 	transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
	// }
}
