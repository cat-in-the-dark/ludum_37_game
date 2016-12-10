using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldRotation : MonoBehaviour {
	public float angleSpeed;

	IDictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
	float currentRotation;
	bool rotating = false;
	Vector2[] roomGroup = null;
	Vector3 axis = Vector3.zero;

	GameObject roomGroupGO;

	// Use this for initialization
	void Start () {
		foreach (GameObject room in GameObject.FindGameObjectsWithTag ("MiniRoom")) {
			rooms [room.name] = room;
		}
		currentRotation = 0f;
		roomGroupGO = new GameObject ("roomGroup");
	}
	
	// Update is called once per frame
	void Update () {
		// ==== X ====
		if (Input.GetKeyDown (KeyCode.K)) {
			StartRotation (
				roomGroup:new Vector2[]{ new Vector2(1,1), new Vector2(1,2), new Vector2(1,3), new Vector2(1,4) }, 
				axis: Vector3.down);
		}

		if (Input.GetKeyDown (KeyCode.J)) {
			StartRotation (
				roomGroup: new Vector2[]{ new Vector2(1,1), new Vector2(1,2), new Vector2(1,3), new Vector2(1,4) }, 
				axis: Vector3.up);
		}

	
		// ==== Y ====
		if (Input.GetKeyDown (KeyCode.U)) {
			StartRotation (
				roomGroup:new Vector2[]{ new Vector2(1,1), new Vector2(1,2), new Vector2(2,1), new Vector2(2,2) }, 
				axis: Vector3.back);
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			StartRotation (
				roomGroup: new Vector2[]{ new Vector2(1,1), new Vector2(1,2), new Vector2(2,1), new Vector2(2,2) }, 
				axis: Vector3.forward);
		}

		// ==== Z ====
		if (Input.GetKeyDown (KeyCode.N)) {
			StartRotation (
				roomGroup:new Vector2[]{ new Vector2(1,2), new Vector2(1,3), new Vector2(2,2), new Vector2(2,3) }, 
				axis: Vector3.left);
		}

		if (Input.GetKeyDown (KeyCode.M)) {
			StartRotation (
				roomGroup:new Vector2[]{ new Vector2(1,2), new Vector2(1,3), new Vector2(2,2), new Vector2(2,3) }, 
				axis: Vector3.right);
		}

		if (rotating) {
			Rotate (roomGroup, axis);
		}
	}


	void StartRotation(Vector2[] roomGroup, Vector3 axis) {
		if (!this.rotating) {
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

	void Rotate(Vector2[] roomGroupIds, Vector3 axis) {
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
			var name = string.Format ("Cube_{0}_{1}", id.x, id.y);
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
