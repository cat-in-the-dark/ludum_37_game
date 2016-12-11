using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlyerMovement : MonoBehaviour {
	public float speed = 6f;
	Vector3 movement;
	Rigidbody playerRigidbody;
	CharacterController characterController;

	bool needLockScreen = true;
	bool cursorLocked = true;
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

	public void setCursorState(bool locked) {
		Debug.Log(string.Format("set retracted: {0}", locked));
		cursorLocked = locked;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Debug.Log ("Hard reset");
			SceneManager.LoadScene (0);
			return;
		}
		if (needLockScreen) {
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			Cursor.lockState = CursorLockMode.None;
		}

		if (cursorLocked) {
			float h = Input.GetAxisRaw ("Horizontal") * sense;
			float v = Input.GetAxisRaw ("Vertical") * sense;

			float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
			transform.Rotate (0, rotLeftRight, 0);

			verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
			verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
			Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
			Move (h, v);
		} else {
			float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
			transform.Rotate (0, rotLeftRight, 0);

			verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
			verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
			Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
			Move (0, 0);
		}
	}

	void Move(float h, float v) {
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		movement = transform.TransformDirection (movement);
		//if(characterController.isGrounded)
			//print("grounded");
		//else
			//print("not grounded");
		//playerRigidbody.MovePosition (transform.position + movement);
		characterController.Move(movement);		
		characterController.Move(Vector3.down * gravity * Time.deltaTime);		
		//playerRigidbody.AddForce (movement, ForceMode.Impulse);
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log (string.Format ("{0} {1}", other.name, other.tag));
		if (other.tag == "Finish") {
			var scene = SceneManager.GetActiveScene ();
			Debug.Log (string.Format ("Level Finished {0} {1}", scene.name, scene.buildIndex));
			Debug.Log (SceneManager.sceneCountInBuildSettings.ToString ());
			if (SceneManager.sceneCountInBuildSettings -1 > scene.buildIndex) {
				Debug.Log ("Next level");
				SceneManager.LoadScene (scene.buildIndex + 1);
			} else {
				Debug.Log ("Go to menu");
				SceneManager.LoadScene (0);
			}
		}
	}
}
