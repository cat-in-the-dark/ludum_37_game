using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void StartGame() {
		Debug.Log ("Game Started");
		SceneManager.LoadScene ("Scenes/Level0", LoadSceneMode.Single);
	}

	public void Start() {
		Cursor.lockState = CursorLockMode.None;
	}
}
