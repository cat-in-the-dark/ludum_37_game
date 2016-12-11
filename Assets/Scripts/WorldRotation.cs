using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldRotation : MonoBehaviour {
	public float angleSpeed;
	public AudioSource rotateSound;

	IDictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
	float currentRotation;
	bool rotating = false;
	int[] roomGroup = null;
	Vector3 axis = Vector3.zero;

	GameObject roomGroupGO;
	GameObject playerGO;
	PlyerMovement movement;
	public GameObject mapPrefab;
	Map mapComponent;

	Vector3 initialMapPos = new Vector3 (0.7f, 0, 2);
	Vector3 deployedMapPos = new Vector3 (0, 0, 1);
	float moveMapSpeed = 5f;
	bool deployingMap = false;
	bool retractingMap = false;
	bool mapRetracted = true;
	float lerpTreshold = 0.02f;
	float lerpProgress = 0f;

	// Use this for initialization
	void Start () {
		foreach (GameObject room in GameObject.FindGameObjectsWithTag ("MiniRoom")) {
			rooms [room.name] = room;
			Debug.Log (room.name);
		}

		playerGO = GameObject.FindGameObjectWithTag ("Player");
		movement = playerGO.GetComponent<PlyerMovement> ();
		GameObject map = (GameObject)Instantiate (mapPrefab, playerGO.transform);
		mapComponent = map.GetComponent<Map> ();
		mapComponent.transform.localPosition = new Vector3 (0.7f, 0, 2);
		mapComponent.transform.localScale = new Vector3 (0.3F, 0.3F, 0.3F);

		currentRotation = 0f;
		roomGroupGO = new GameObject ("roomGroup");
	}
	
	// Update is called once per frame
	void Update () {
		// ==== X ====
		if (Input.GetKeyDown (KeyCode.K)) {
			StartRotation (
				roomGroup:new int[]{1,2,3,4}, 
				axis: Vector3.down);
		}

		if (Input.GetKeyDown (KeyCode.J)) {
			StartRotation (
				roomGroup:new int[]{1,2,3,4}, 
				axis: Vector3.up);
		}

	
		// ==== Y ====
		if (Input.GetKeyDown (KeyCode.U)) {
			StartRotation (
				roomGroup:new int[]{2,3,6,7}, 
				axis: Vector3.back);
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			StartRotation (
				roomGroup:new int[]{2,3,6,7},  
				axis: Vector3.forward);
		}

		// ==== Z ====
		if (Input.GetKeyDown (KeyCode.N)) {
			StartRotation (
				roomGroup:new int[]{1,2,5,6}, 
				axis: Vector3.left);
		}

		if (Input.GetKeyDown (KeyCode.M)) {
			StartRotation (
				roomGroup:new int[]{1,2,5,6}, 
				axis: Vector3.right);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (mapRetracted && !retractingMap) {
				deployingMap = true;
			} else if (!mapRetracted && !deployingMap) {
				retractingMap = true;
			}
		}

		if (mapComponent != null) {
			if (Input.GetKeyDown (KeyCode.Alpha0)) {
				mapComponent.setUserPresent (0);
			}

			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				mapComponent.setUserPresent (1);
			}

			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				mapComponent.setUserPresent (2);
			}

			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				mapComponent.setUserPresent (3);
			}

			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				mapComponent.setUserPresent (4);
			}

			if (Input.GetKeyDown (KeyCode.Alpha5)) {
				mapComponent.setUserPresent (5);
			}

			if (Input.GetKeyDown (KeyCode.Alpha6)) {
				mapComponent.setUserPresent (6);
			}

			if (Input.GetKeyDown (KeyCode.Alpha7)) {
				mapComponent.setUserPresent (7);
			}
		}

		if (rotating) {
			Rotate (roomGroup, axis);
		}

		if (deployingMap) {
			mapComponent.transform.localPosition = Vector3.Slerp ( initialMapPos, deployedMapPos, lerpProgress);
			lerpProgress += moveMapSpeed * Time.deltaTime;
			if (Mathf.Abs (lerpProgress - 1) < lerpTreshold) {
				mapComponent.transform.localPosition = deployedMapPos;
				Debug.Log ("deployed");
				deployingMap = false;
				mapRetracted = false;
				lerpProgress = 0;
				movement.setCursorState (mapRetracted);
			}
		}

		if (retractingMap) {
			mapComponent.transform.localPosition = Vector3.Slerp (deployedMapPos, initialMapPos, lerpProgress);
			lerpProgress += moveMapSpeed * Time.deltaTime;
			print (lerpProgress);
			if (Mathf.Abs (lerpProgress - 1) < lerpTreshold) {
				mapComponent.transform.localPosition = initialMapPos;
				Debug.Log ("retracted");
				retractingMap = false;
				mapRetracted = true;
				lerpProgress = 0;
				movement.setCursorState (mapRetracted);
			}
		}
	}


	void StartRotation(int[] roomGroup, Vector3 axis) {
		if (!this.rotating) {
			if (rotateSound != null) {
				rotateSound.Play ();
			}
			this.rotating = true;
			this.currentRotation = 0f;
			this.roomGroup = roomGroup; 
			this.axis = axis;
			foreach (var room in rooms) {
				room.Value.transform.SetParent (null);
			}
			roomGroupGO.transform.eulerAngles = Vector3.zero;
		}
	}

	void Rotate(int[] roomGroupIds, Vector3 axis) {
		if (roomGroupIds == null) return;
		if (Mathf.Abs (currentRotation) >= 90.0f) {
			roomGroupGO.transform.eulerAngles = axis * 90.0f;
			rotating = false;
			return;
		} else {
			rotating = true;
		}
		var angleDelta = angleSpeed * Time.deltaTime;
		currentRotation += angleDelta;
		var roomGroup = new List<GameObject>();
		var center = Vector3.zero;
		foreach (var id in roomGroupIds) {
			var name = string.Format ("Cube{0}", id);
			GameObject room = rooms [name];
			roomGroup.Add (room);
			center.x += room.transform.position.x;
			center.y += room.transform.position.y;
			center.z += room.transform.position.z;
		}

		var scale = 1.0f / roomGroupIds.Length;
		center.Scale (new Vector3(scale, scale, scale));

		roomGroupGO.transform.position = center;
		foreach (var room in roomGroup) {
			room.transform.parent = roomGroupGO.transform;
		}

		roomGroupGO.transform.Rotate (axis, angleDelta);
	}
}
