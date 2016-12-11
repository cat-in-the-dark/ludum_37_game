using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void StartGame() {
		Debug.Log ("Game Started");
		SceneManager.LoadScene (1, LoadSceneMode.Single);
	}

	public void Start() {
		Cursor.lockState = CursorLockMode.None;
	}
}
