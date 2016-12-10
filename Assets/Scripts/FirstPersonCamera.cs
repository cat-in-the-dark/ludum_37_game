using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour {
	public Transform target;
	public float smoothing = 5f;

	Vector3 offset;

	void Start () {
		offset = transform.position - target.position;
	}

	void FixedUpdate () {
		Vector3 targetCamPos = target.position + offset;

		// Smoothly interpolate between the camera's current position and it's target position.
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
		transform.localEulerAngles = target.localEulerAngles;
	}
}
