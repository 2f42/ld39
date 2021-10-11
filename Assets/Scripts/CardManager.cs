using UnityEngine;
using System.Collections;
using System;

public class CardManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (FillDeck) {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    //Debug.Log(i.ToString() + j.ToString());
                    deck.Add(new Vector2(i, j));
                }
            }
        }
	}

    public bool FillDeck;
    ArrayList deck = new ArrayList();

    // Update is called once per frame
    void Update () {
	
	}

    public Vector2 GetRandomCard() {
        int cardPos = UnityEngine.Random.Range(0, deck.Count);
        //Debug.Log(cardPos);
        return (Vector2) deck[cardPos];
    }

    public string GetRandomCardString() {
        return CardToReadable(GetRandomCard());
    }

    public string DealRandomCardString() {
        return CardToReadable(DealRandomCard());
    }

    public string CardToReadable(Vector2 card) {
        string suit = "";
        switch ((int) card.x) {
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
        switch ((int) card.y) {
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
                value = (card.y - 1).ToString();
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
        return value + " of " + suit;
    }

    public Vector2 DealRandomCard() {
        Vector2 card = GetRandomCard();
        deck.RemoveAt(deck.IndexOf(card));
        return card;
    }

    public Vector2 DealCard(int index) {
        Vector2 card = (Vector2) deck[index];
        deck.RemoveAt(index);
        return card;
    }

    public void RemoveCard(Vector2 card) {
        deck.Remove(card);
    }

    public void AddCard(Vector2 card) {
        deck.Add(card);
    }

    public void PickupRandomFromDeck(GameObject otherDeck) {
        CardManager other = otherDeck.GetComponent<CardManager>();
        AddCard(other.DealRandomCard());
    }

    public void MergeDeckInto(GameObject otherDeck) {
        CardManager other = otherDeck.GetComponent<CardManager>();
        foreach (Vector2 card in deck) {
            other.AddCard(card);
        }
    }

    public void MergeDeckFrom(GameObject otherDeck) {
        CardManager other = otherDeck.GetComponent<CardManager>();
        foreach (Vector2 card in other.GetDeck()) {
            AddCard(card);
        }
    }

    public ArrayList GetDeck() {
        return deck;
    }

    public int CardCount() {
        return deck.Count;
    }

    public bool IsEmpty() {
        return deck.Count == 0;
    }

}
