using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Map : MonoBehaviour {

	public GameObject mapSegmentPrefab;

	Dictionary <int, Segment> Segments = new Dictionary<int, Segment>();

	int touchIndex = -1;
	Vector3 touchDownPos;

	public float horizontalSpeed = 2.0F;
	public float verticalSpeed = 2.0F;

	Vector3 axis;
	GameObject segmentGroupGo;
	float currentRotation;
	bool isRotatedByUser;

	// Use this for initialization
	void Start () {
		touchDownPos = Vector3.zero;
		currentRotation = 0f;
		isRotatedByUser = false;
		segmentGroupGo = new GameObject ("segmentGroup");
	}

	public void initSegments(Vector3 initialPos, Vector3 initialRot) {
		for (int i = 0; i < 8; i++) {
			GameObject segmentObj = Instantiate (mapSegmentPrefab.gameObject,
				new Vector3 (i % 2, (i / 4) % 2, (i / 2) % 2), Quaternion.identity) as GameObject;
			Segment segment = segmentObj.GetComponent<Segment>();
			segment.index = i;

			segment.setTouchDownCallback (onTouchedDown);
			segment.setTouchUpCallback (onTouchedUp);

			Segments.Add (i, segment);
		}

		connectNeighbours (new int[] { 0, 1, 5, 4 }, Segment.DIRECTION.XY);
		connectNeighbours (new int[] { 2, 6, 7, 3 }, Segment.DIRECTION.XY);

		connectNeighbours (new int[] { 0, 2, 3, 1 }, Segment.DIRECTION.XZ);
		connectNeighbours (new int[] { 5, 7, 6, 4 }, Segment.DIRECTION.XZ);

		connectNeighbours (new int[] { 0, 4, 6, 2 }, Segment.DIRECTION.YZ);
		connectNeighbours (new int[] { 1, 3, 7, 5 }, Segment.DIRECTION.YZ);
	}

	public void setUserPresent(int index) {
		if (Segments.Count >= index - 1) {
			foreach (Segment segment in Segments.Values) {
				if (segment.hasUser) {
					Debug.Log (string.Format ("user present {0} -> {1}", segment.index, index));
					segment.hasUser = false;
					break;
				}
			}

			Segments [index].hasUser = true;
		}
	}

	void connectNeighbours(int[] indexes, Segment.DIRECTION type) {
		int i;
		for (i = 0; i < 3; i++) {
			Segments [indexes[i]].neighbour [(int)type] = Segments[indexes[i + 1]];
		}

		Segments [indexes [i]].neighbour [(int)type] = Segments[indexes[0]];
	}

	void onTouchedDown(int index) {
		Debug.Log(string.Format("{0} {1}", "tut", index));
		touchIndex = index;
		touchDownPos = Input.mousePosition;
		Segment touchedSegment = Segments [index];
		Segment neighbourWithUser = null;
		int type;
		for (type = 0; type < 3; type++) {
			if (touchedSegment.neighbour [type].hasUser) {
				neighbourWithUser = touchedSegment.neighbour [type];
			}
		}

		if (neighbourWithUser == null) {
			return;
		}

		List<Segment> rotationChain = null;
		for (type = 0; type < 3; type++) {
			if (!touchedSegment.hasNeighbourInChain(neighbourWithUser, type)) {
				rotationChain = touchedSegment.getNeighbourChain (type);
				break;
			}
		}

		if (rotationChain == null) {
			Debug.LogError ("WTF all chains have segment with user as neighbour");
			return;
		}

		List<Segment> staticChain = neighbourWithUser.getNeighbourChain (type);

		Vector3 rotationCenter = calcCenter (rotationChain);
		Vector3 staticCenter = calcCenter (staticChain);

		axis = staticCenter - rotationCenter;

		StartRotation (rotationChain, rotationCenter, axis);
	}

	Vector3 calcCenter(List<Segment> segmentList) {
		Vector3 center = Vector3.zero;
		foreach (Segment segment in segmentList) {
			center.x += segment.transform.position.x;
			center.y += segment.transform.position.y;
			center.z += segment.transform.position.z;
		}

		var scale = 1.0f / segmentList.Count;
		center.Scale (new Vector3(scale, scale, scale));

		return center;
	}

	void StartRotation(List<Segment> segmentList, Vector3 center, Vector3 axis) {
		if (!this.isRotatedByUser) {
			this.isRotatedByUser = true;
			this.currentRotation = 0f;
			this.axis = axis;
			foreach (Segment segment in Segments.Values) {
				segment.transform.SetParent (null);
			}

			segmentGroupGo.transform.position = center;
			segmentGroupGo.transform.eulerAngles = Vector3.zero;
			foreach (Segment segment in segmentList) {
				segment.transform.parent = segmentGroupGo.transform;
			}
		}
	}

	void onTouchedUp(int index) {
		//Debug.Log(string.Format("{0} {1}", "tut", index));
		touchIndex = -1;
		isRotatedByUser = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isRotatedByUser) {
			float h = horizontalSpeed * Input.GetAxis("Mouse X");
			float v = verticalSpeed * Input.GetAxis("Mouse Y");
			segmentGroupGo.transform.Rotate (axis, h + v);
		}
	}
}
