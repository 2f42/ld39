using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CustomDifficulty : MonoBehaviour {

	// Use this for initialization
	void Start () { 
        difficultyControl = GameObject.FindGameObjectWithTag("DifficultyControl").GetComponent<DifficultyControl>();
    }

    public int humans;
    public int turns;
    public int power;

    public GameObject humanInput;
    public GameObject powerInput;
    public GameObject turnsInput;

    public DifficultyControl difficultyControl;

	// Update is called once per frame
	void Update () {
        humans = Int32.Parse(humanInput.GetComponent<InputField>().text);
        turns = Int32.Parse(turnsInput.GetComponent<InputField>().text);
        power = Int32.Parse(powerInput.GetComponent<InputField>().text);
    }

    public void SetDifficulty() {
        difficultyControl.SetHumans(humans);
        difficultyControl.SetPower(power);
        difficultyControl.SetTurns(turns);
    }
}
