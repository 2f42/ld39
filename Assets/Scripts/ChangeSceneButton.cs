using UnityEngine;
using System.Collections;

public class ChangeSceneButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}

    public string targetScene;
	
	// Update is called once per frame
	void Update () {
	    
	}

    // When button is pressed
    public void OnClick() {
        GameObject.FindGameObjectWithTag("ClickSound").GetComponent<AudioSource>().Play();
        UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
    }
}
