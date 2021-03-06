﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotation : MonoBehaviour {
	List<GameObject> rooms = new List<GameObject>();
	bool rotating = false;
	float currentRotation = 0f;
	GameObject roomGroupGO;
	public float angleSpeed = 60f;
	public float rotateDirection = 1f;
	GameObject[] roomGroup;
	Vector3 axis;

	void Start () {
		foreach (Transform room in transform) {
			if (room.tag != "MiniRoom") continue;
			rooms.Add(room.gameObject);
		}

		roomGroupGO = new GameObject ("cubesGroup");
		roomGroupGO.transform.SetParent(this.transform);
	}


	void Update () {
		// ==== X ====
		if (Input.GetKeyDown (KeyCode.K)) {
			StartRotation (axis: Vector3.up, direction: 1f);
		}

		if (Input.GetKeyDown (KeyCode.J)) {
			StartRotation (axis: Vector3.up, direction: -1f);
		}

	
		// ==== Y ====
		if (Input.GetKeyDown (KeyCode.U)) {
			StartRotation (axis: Vector3.back, direction: 1f);
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			StartRotation (axis: Vector3.back, direction: -1f);
		}

		// ==== Z ====
		if (Input.GetKeyDown (KeyCode.N)) {
			StartRotation (axis: Vector3.left, direction: 1f);
		}

		if (Input.GetKeyDown (KeyCode.M)) {
			StartRotation (axis: Vector3.left, direction: -1f);
		}

		if (rotating) {
			Rotate (roomGroup, axis);
		}
	}

	void StartRotation(Vector3 axis, float direction) {
		if (!this.rotating) {
			this.rotateDirection = direction;
			this.rotating = true;
			this.currentRotation = 0f;
			foreach (var room in rooms) {
				room.transform.SetParent (this.transform);
			}
			roomGroupGO.transform.eulerAngles = Vector3.zero;
			roomGroup = Select(axis);
			this.axis = axis;
		}
	}

	void Rotate(GameObject[] cubes, Vector3 axis) {
		if (Mathf.Abs (currentRotation) >= 90.0f) {
			roomGroupGO.transform.eulerAngles = axis * 90.0f * rotateDirection;
			rotating = false;
			return;
		} else {
			rotating = true;
		}
		var angleDelta = angleSpeed * Time.deltaTime;
		currentRotation += angleDelta;
		var roomGroup = new List<GameObject>();
		foreach (var cube in cubes) {
			roomGroup.Add (cube);
		}

		foreach (var room in roomGroup) {
			room.transform.parent = roomGroupGO.transform;
		}

		roomGroupGO.transform.Rotate (axis, angleDelta * rotateDirection);
	}

	GameObject[] Select(Vector3 axis) {
		List<GameObject> cubes = new List<GameObject>();
		
		foreach (var room in rooms) {
			if (Vector3.Dot(room.transform.localPosition, axis) > 0) {
				cubes.Add(room);
			}
		}

		return cubes.ToArray();
	}
}
