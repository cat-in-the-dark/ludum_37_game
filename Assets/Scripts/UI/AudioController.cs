using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour {
	public static AudioController instance;

	public AudioSource background;

	void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	// Use this for initialization
	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

		DontDestroyOnLoad (this.gameObject);
	}
	
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		Debug.Log(scene.name);
		if (scene.name == "LogoScene" || scene.name == "Scenes/MainMenu") {
			background.Stop ();
		} else {
			if (!background.isPlaying) {
				background.Play ();
			}
		}
	}
}
