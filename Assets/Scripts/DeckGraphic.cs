using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeckGraphic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if (cards == null) {
            cards = gameObject.GetComponent<CardManager>();
            image = gameObject.GetComponent<Image>();
            currentCardCount = cards.CardCount();
            text = gameObject.GetComponentInChildren<Text>();
        }
	}

    CardManager cards;
    Image image;
    int currentCardCount;
    Text text;

	// Update is called once per frame
	void Update () {
        bool dirty = false;
	    if (cards.CardCount() != currentCardCount) {
            currentCardCount = cards.CardCount();
            dirty = true;
        }

        if (dirty) {
            if (currentCardCount < 1) {
                image.enabled = false;
            } else {
                image.enabled = true;
            }
            text.text = currentCardCount.ToString() + " cards left in the deck";
        }
	}
}
