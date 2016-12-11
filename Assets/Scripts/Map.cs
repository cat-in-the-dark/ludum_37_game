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
	float CurrentRotation = 0f;

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
		TargetGo = new GameObject ("TargetGO");
		OldGo = new GameObject ("OldGo");
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
	}

	public void setUserPresent(int index) {
		if (Segments.Count >= index - 1) {
			foreach (Segment segment in Segments.Values) {
				if (segment.hasUser) {
					segment.hasUser = false;
					break;
				}
			}

			Segments [index].hasUser = true;
		}
	}

	void onTouchedDown(int index) {
		touchIndex = index;
		touchDownPos = Input.mousePosition;
		Segment touchedSegment = Segments [index];
		Segment neighbourWithUser = null;
		foreach (Segment segment in Segments.Values) {
			if (segment.isNeighbour(touchedSegment)) {
				if (segment.hasUser) {
					neighbourWithUser = segment;
					break;
				}
			}
		}

		if (neighbourWithUser == null) {
			return;
		}

		List<Segment> rotationSlice = new List<Segment>();
		rotationSlice.Add (touchedSegment);

		//TODO: make something less stupid pls
		foreach (Segment segment in Segments.Values) {
			if ((segment.index != touchedSegment.index)
				&& (segment.isNeighbour(touchedSegment))
				&& (!segment.isNeighbour(neighbourWithUser))
				&& (segment.index != neighbourWithUser.index)
				&& (!rotationSlice.Contains(segment))) {
				rotationSlice.Add (segment);
			}
		}

		foreach (Segment segment in Segments.Values) {
			if (segment.isNeighbour(rotationSlice[1])
				&& (segment.isNeighbour(rotationSlice[2]))
				&& (!rotationSlice.Contains(segment))) {
				rotationSlice.Add (segment);
			}
		}

		List<Segment> staticSlice = new List<Segment> ();
		foreach (Segment segment in Segments.Values) {
			if (!rotationSlice.Contains (segment)) {
				staticSlice.Add (segment);
			}
		}

		RotationCenter = calcCenter (rotationSlice);
		StaticCenter = calcCenter (staticSlice);

		RotatingSegments = rotationSlice;
		StaticSegments = staticSlice;

		axis = StaticCenter - RotationCenter;

		StartRotation (rotationSlice, RotationCenter, axis);
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
		}
	}

	void onTouchedUp(int index) {
		if (!isRotatedByUser) {
			return;
		}

		touchIndex = -1;
		isRotatedByUser = false;

		TargetGo.transform.position = RotationCenter;
		TargetGo.transform.eulerAngles = Vector3.zero;
		float rotation = Mathf.RoundToInt (CurrentRotation / 90) * 90f;
		TargetGo.transform.Rotate (axis, rotation);

		OldGo.transform.position = SegmentGroupGo.transform.position;
		OldGo.transform.rotation = SegmentGroupGo.transform.rotation;
		isRotatedFinishing = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isRotatedByUser) {
			float h = horizontalSpeed * Input.GetAxis ("Mouse X");
			float v = verticalSpeed * Input.GetAxis ("Mouse Y");
			var angleDelta = h + v;
			CurrentRotation += angleDelta;

			if (CurrentRotation < 0) {
				CurrentRotation += 360;
			}

			CurrentRotation %= 360;
			SegmentGroupGo.transform.Rotate (axis, angleDelta);
		} else if (isRotatedFinishing) {
			SegmentGroupGo.transform.rotation = 
				Quaternion.Lerp (OldGo.transform.rotation, TargetGo.transform.rotation, LerpProgerss);
			LerpProgerss += Time.deltaTime * RotationLerpSpeed;
			if (Mathf.Abs(LerpProgerss - 1) < lerpTreshold) {
				SegmentGroupGo.transform.rotation = TargetGo.transform.rotation;
				isRotatedFinishing = false;
				LerpProgerss = 0f;
				CurrentRotation = 0f;
			}
		}
	}
}
