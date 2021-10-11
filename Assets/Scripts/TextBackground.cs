using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBackground : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public Text textToCopy;
    public Text thisText;

	// Update is called once per frame
	void Update () {
        thisText.text = textToCopy.text;
	}
}
