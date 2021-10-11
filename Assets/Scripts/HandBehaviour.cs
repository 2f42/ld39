using UnityEngine;
using System.Collections;
using System;

public class HandBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        cards = gameObject.GetComponent<CardManager>();
        discard = GameObject.FindGameObjectWithTag("DiscardPile").GetComponent<CardManager>();
        deck = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManager>();
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameControl>();
    }

    CardManager cards;
    CardManager discard;
    CardManager deck;
    GameControl controller;
    public AudioSource soundPlay;
    public AudioSource soundPickup;
    public GameObject cardPrefab;

    bool initialised = false;

	// Update is called once per frame
	void Update () {
	    if (!initialised) {
            for (int i=0; i < controller.startingCards; i++) {
                AddCard(deck.DealRandomCard(), true);
            }
            initialised = true;
        }
	}

    public string VecToReadable(Vector2 card) {
        string suit = "";
        switch ((int)card.x)
        {
            case 0:
                suit = "Spades";
                break;
            case 1:
                suit = "Clubs";
                break;
            case 2:
                suit = "Diamonds";
                break;
            case 3:
                suit = "Hearts";
                break;
        }
        string value = "";
        switch ((int)card.y)
        {
            case 0:
                value = "Ace";
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
                value = (card.y + 1).ToString();
                break;
            case 10:
                value = "Jack";
                break;
            case 11:
                value = "Queen";
                break;
            case 12:
                value = "King";
                break;
        }

        Debug.Log("converted to string");

        return suit + "x" + value;
    }

    public Vector2 ReadableToVec(string cardString) {
        Debug.Log(cardString);
        string[] splitRead = cardString.Split('x');
        int x = 0;
        int y = 0;

        switch (splitRead[0]) {
            case "Spades":
                x = 0;
                break;
            case "Clubs":
                x = 1;
                break;
            case "Diamonds":
                x = 2;
                break;
            case "Hearts":
                x = 3;
                break;
        }

        switch (splitRead[1]) {
            case "Ace":
                y = 0;
                break;
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
            case "10":
                y = Int32.Parse(splitRead[1]) - 1;
                break;
            case "Jack":
                y = 10;
                break;
            case "Queen":
                y = 11;
                break;
            case "King":
                y = 12;
                break;
        }

        Debug.Log("converted to vector");

        return new Vector2(x, y);
    }

    public void AddCard(Vector2 card, bool alert=true, bool playSound=false) {
        string readableCard = VecToReadable(card);
        string[] splitRead = readableCard.Split('x');
        string suit = splitRead[0];
        string value = splitRead[1];

        GameObject newCard = Instantiate(cardPrefab);
        CardBehaviour cardBeh = newCard.GetComponent<CardBehaviour>();

        cardBeh.SetSuit(suit);
        cardBeh.SetValue(value);

        newCard.transform.SetParent(this.transform);
        newCard.transform.localScale = Vector2.one;

        if (alert) {
            AlertListManager.NewAlert("Picked up a new card: " + value + " of " + suit, suit);
        }

        if (playSound) {
            soundPickup.Play();
        }
    }

    public void PlayCard(Vector2 card, bool alert=true) {
        Debug.Log("begin playing card");
        discard.AddCard(card);
        Debug.Log("added card to discard");
        cards.RemoveCard(card);
        Debug.Log("removed card from hand");
        string[] cardReadable = VecToReadable(card).Split('x');
        foreach (string s in cardReadable)
        {
            Debug.Log(s);
        }
        if (alert) {
            AlertListManager.NewAlert("You played the " + cardReadable[1]  + " of " + cardReadable[0], cardReadable[0]);
        }
        Debug.Log("Finished playing card");
        controller.PlayCard();
    }

    public void PlayCard(string card, bool alert=true) {
        PlayCard(ReadableToVec(card), alert);
    }
}
