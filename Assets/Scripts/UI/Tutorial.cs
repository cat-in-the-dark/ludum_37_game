using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
	public GameObject target;
	public KeyCode keyCode;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (keyCode)) {
			target.SetActive(false);
		}
	}
}
