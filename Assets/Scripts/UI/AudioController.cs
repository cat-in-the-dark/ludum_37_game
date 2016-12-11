using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour {
	public static AudioController instance;

	public AudioSource background;

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
	
	void OnLevelWasLoaded(int level) {
		if (level == 0) {
			background.Stop ();
		} else {
			if (!background.isPlaying) {
				background.Play ();
			}
		}
	}
}
