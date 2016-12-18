using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AwaitSceneChanger : MonoBehaviour {
	public float time;
	public string nextSceneName;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(time);
		SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
	}
}
