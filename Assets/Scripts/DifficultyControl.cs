using UnityEngine;
using System.Collections;

public class DifficultyControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}

    public int startingTurns;
    public int startingPower;
    public int startingHumans;

	// Update is called once per frame
	void Update () {
	    
	}

    public void SetTurns(int a) {
        startingTurns = a;
    }

    public void SetPower(int a) {
        startingPower = a;
    }

    public void SetHumans(int a) {
        startingHumans = a;
    }
}
