using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Segment : MonoBehaviour {

	public Segment[] neighbour = new Segment[3];
	public bool hasUser = false;
	public int index = -1;

	public enum DIRECTION {
		XY = 0,
		XZ = 1,
		YZ = 2
	}

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
		}
	}

	void OnMouseUp() {
		if (touchUpCallback != null) {
			touchUpCallback (index);
		}
	}

	public bool hasNeighbourInChain (Segment target, int type) {
		Segment curNeighbour = this.neighbour[type];
		while (curNeighbour.index != this.index) {
			if (curNeighbour.index == target.index) {
				return true;
			}

			curNeighbour = curNeighbour.neighbour[type];
		}

		return false;
	}

	public List<Segment> getNeighbourChain(int type) {
		List<Segment> neighbourChain = new List<Segment> ();
		neighbourChain.Add (this);
		Segment currNeighbour = this.neighbour[type];
		while (currNeighbour.index != this.index) {
			neighbourChain.Add (currNeighbour);
			currNeighbour = currNeighbour.neighbour [type];
		}

		return neighbourChain;
	}

	public Segment findNearest(List<Segment> segments) {
		var minDist = Mathf.Infinity;
		Segment nearestSegment = segments[0];
		foreach (Segment segment in segments) {
			var dist = (segment.gameObject.transform.position
			           - this.gameObject.transform.position).magnitude;
			if (dist < minDist) {
				minDist = dist;
				nearestSegment = segment;
			}
		}

		return nearestSegment;
	}
}
