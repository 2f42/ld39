using UnityEngine;
using System.Collections;

public class TestAlert : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<HandBehaviour>();
    }

    public HandBehaviour hand;

	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick() {
        Vector2 card = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManager>().DealRandomCard();
        hand.AddCard(card, true);
    }
}
