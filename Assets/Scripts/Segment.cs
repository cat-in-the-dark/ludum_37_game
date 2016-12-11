using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Segment : MonoBehaviour {

	public bool hasUser = false;
	public int index = -1;

	Action<int> tounchDownCallback;
	Action<int> touchUpCallback;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setTouchDownCallback(Action<int> callback) {
		tounchDownCallback = callback;
	}

	public void setTouchUpCallback(Action<int> callback) {
		touchUpCallback = callback;
	}

	void OnMouseDown() {
		if (tounchDownCallback != null) {
			tounchDownCallback (index);
		} else {
			Debug.Log ("WTF callback is null");
		}
	}

	void OnMouseUp() {
		if (touchUpCallback != null) {
			touchUpCallback (index);
		} else {
			Debug.Log ("WTF callback is null");
		}
	}

	public bool isNeighbour(Segment segment) {
		return (this.index != segment.index
			&& (this.transform.localPosition - segment.transform.localPosition).magnitude <= 1.01f);
	}

	public Segment findNearest(List<Segment> segments) {
		var minDist = Mathf.Infinity;
		Segment nearestSegment = segments[0];
		foreach (Segment segment in segments) {
			var dist = (segment.gameObject.transform.localPosition
			           - this.gameObject.transform.localPosition).magnitude;
			if (dist < minDist) {
				minDist = dist;
				nearestSegment = segment;
			}
		}

		return nearestSegment;
	}
}
