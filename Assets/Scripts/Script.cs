using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Script : MonoBehaviour {

	public GameObject mapPrefab;
	Map mapComponent;

	// Use this for initialization
	void Start () {
		GameObject map = (GameObject)Instantiate (mapPrefab);
		mapComponent = map.GetComponent<Map>();
		mapComponent.initSegments (new Vector3(0, 0, 0), new Vector3(0, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
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
	}
}
