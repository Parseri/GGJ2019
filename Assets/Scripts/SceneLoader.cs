using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void onStartClicked() {
        int scene = SceneManager.GetActiveScene().buildIndex+1;
		Debug.Log("Scenes: " + SceneManager.sceneCountInBuildSettings);
        if (scene >= SceneManager.sceneCountInBuildSettings) scene = 0;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
