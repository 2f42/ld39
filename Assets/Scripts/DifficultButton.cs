using UnityEngine;
using System.Collections;

public class DifficultButton : MonoBehaviour {

    // Use this for initialization
    void Start() {
        difficultyControl = GameObject.FindGameObjectWithTag("DifficultyControl").GetComponent<DifficultyControl>();
    }

    public DifficultyControl difficultyControl;
    public int humans;
    public int power;
    public int turns;

    // Update is called once per frame
    void Update() {

    }

    public void SetDifficulty() {
        difficultyControl.SetHumans(humans);
        difficultyControl.SetPower(power);
        difficultyControl.SetTurns(turns);
    }
}
