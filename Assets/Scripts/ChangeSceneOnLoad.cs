using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneOnLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("_MAINMENU");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
