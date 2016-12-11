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

	public float RotationLerpSpeed = 20f;
	float lerpTreshold = 0.02f;
	float LerpProgerss = 0f;

	Vector3 axis;
	GameObject SegmentGroupGo;
	GameObject TargetGo;
	GameObject OldGo;
	bool isRotatedByUser;
	bool isRotatedFinishing;
	List<Segment> RotatingSegments;
	List<Segment> StaticSegments;
	Vector3 RotationCenter;
	Vector3 StaticCenter;
	Transform TargetTransform;

	// Use this for initialization
	void Start () {
		touchDownPos = Vector3.zero;
		isRotatedByUser = false;
		isRotatedFinishing = false;
		RotationCenter = Vector3.zero;
		StaticCenter = Vector3.zero;
		SegmentGroupGo = new GameObject ("segmentGroup");
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
				break;
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

		RotationCenter = calcCenter (rotationChain);
		StaticCenter = calcCenter (staticChain);

		RotatingSegments = rotationChain;
		StaticSegments = staticChain;

		axis = StaticCenter - RotationCenter;

		StartRotation (rotationChain, RotationCenter, axis);
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
		if (!this.isRotatedByUser && !isRotatedFinishing) {
			this.isRotatedByUser = true;
			this.axis = axis;
			foreach (Segment segment in Segments.Values) {
				segment.transform.SetParent (null);
			}

			SegmentGroupGo.transform.position = center;
			SegmentGroupGo.transform.eulerAngles = Vector3.zero;
			foreach (Segment segment in segmentList) {
				segment.transform.parent = SegmentGroupGo.transform;
			}
		} else {
			Debug.Log ("Too early to rotate");
		}
	}

	void onTouchedUp(int index) {
		//Debug.Log(string.Format("{0} {1}", "tut", index));
		if (!isRotatedByUser) {
			return;
		}

		touchIndex = -1;
		isRotatedByUser = false;
		Segment targetRotationSegment = RotatingSegments [0];
		Segment nearestInStatic = targetRotationSegment.findNearest (StaticSegments);
		//Debug.Log (string.Format ("nearest to {0} is {1}", RotatingSegments [0].index, nearestInStatic.index));
		int rotatedBy = 0;
		foreach (Segment rotatingSegment in RotatingSegments) {
			bool flag = false;
			for (int type = 0; type < 3; type++) {
				if (rotatingSegment.neighbour [type].index == nearestInStatic.index) {
					flag = true;
					break;
				}
			}

			if (flag) {
				break;
			}

			rotatedBy++;
		}

		TargetGo = new GameObject ("TargetGO");
		TargetGo.transform.position = RotationCenter;
		TargetGo.transform.eulerAngles = Vector3.zero;
		TargetGo.transform.Rotate (axis, rotatedBy * 90);

		OldGo = new GameObject ("OldGo");
		OldGo.transform.position = SegmentGroupGo.transform.position;
		OldGo.transform.rotation = SegmentGroupGo.transform.rotation;
		isRotatedFinishing = true;

		Debug.Log (string.Format ("rotated by {0}", rotatedBy));
	}
	
	// Update is called once per frame
	void Update () {
		if (isRotatedByUser) {
			float h = horizontalSpeed * Input.GetAxis ("Mouse X");
			float v = verticalSpeed * Input.GetAxis ("Mouse Y");
			var angleDelta = h + v;

			SegmentGroupGo.transform.Rotate (axis, angleDelta);
		} else if (isRotatedFinishing) {
			SegmentGroupGo.transform.rotation = 
				Quaternion.Lerp (OldGo.transform.rotation, TargetGo.transform.rotation, LerpProgerss);
			LerpProgerss += Time.deltaTime * RotationLerpSpeed;
			if (Mathf.Abs(LerpProgerss - 1) < lerpTreshold) {
				isRotatedFinishing = false;
				LerpProgerss = 0f;
				Debug.Log ("finish rotation");
			}
		}
	}
}
