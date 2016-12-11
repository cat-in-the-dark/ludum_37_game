using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour {
	public Transform target;
	public float smoothing = 5f;
	float rotationX = 0f;
	float rotationY = 0f;

	public Vector3 offset = new Vector3(0.1f, -0.1f, 0.1f);

	void Start () {
		//offset = transform.position - target.position;
	}

	void FixedUpdate () {
		Vector3 targetCamPos = target.position;// + offset;

		// Smoothly interpolate between the camera's current position and it's target position.
		//transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
		transform.position = targetCamPos - offset;
		transform.localEulerAngles = target.localEulerAngles;

		float deltaRotationX = Input.GetAxis ("Mouse X");
		float deltaRotationY = -Input.GetAxis ("Mouse Y");

		Rotate (deltaRotationX, deltaRotationY);
	}

	void Rotate(float deltaRotationX, float deltaRotationY) {
		rotationX += deltaRotationY; //  YES, IT IS SO! >_<
		rotationY += deltaRotationX;
		transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
	}
}
