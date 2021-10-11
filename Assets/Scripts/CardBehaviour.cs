using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CardBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	// Use this for initialization
	void Awake () {
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<HandBehaviour>();
        deck = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManager>();
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameControl>();
    }

    string suit;
    string value;
    string card_string;
    string toolTip;
    bool specialGraphic = false;
    HandBehaviour hand;
    CardManager deck;
    GameControl controller;
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Button>().interactable = controller.canPlayCard();
	}

    public void SetSuit(string newSuit) {
        suit = newSuit;
        card_string = suit + "x" + value;
        Transform suitContainer = transform.GetChild(1);
        Sprite sprite = Resources.Load<Sprite>(suit);
        if (sprite != null) {
            suitContainer.GetComponent<Image>().sprite = sprite;
        } else {
            suitContainer.gameObject.SetActive(false);
        }
        UpdateFace();
    }

    public void SetValue(string newValue) {
        value = newValue;
        card_string = suit + "x" + value;
        Text textContainer = transform.GetComponentInChildren<Text>();
        string textToUse;
        if (value.Length > 2) {
            textToUse = value.Substring(0, 1);
            specialGraphic = (value != "Ace");
        } else {
            textToUse = value;
            specialGraphic = false;
        }
        textContainer.text = textToUse;
        UpdateFace();
    }

    public void UpdateFace() {
        Transform faceContainer = gameObject.transform.GetChild(0);
        Sprite texture;
        if (specialGraphic) {
            texture = Resources.Load<Sprite>(card_string);
        } else {
            texture = Resources.Load<Sprite>(suit);
        }

        if (texture != null) {
            faceContainer.GetComponent<Image>().sprite = texture;
        } else {
            faceContainer.gameObject.SetActive(false);
        }

        UpdateTooltip();
    }

    public void UpdateTooltip() {
        if (value == "Ace") {
            toolTip = "Do the same thing as the last card played";
        } else if (value == "2") {
            toolTip = "Pickup 2 more cards";
        } else if (value == "8" || value == "7") {
            toolTip = "Gain 2 more turns";
        } else if (value == "10") {
            toolTip = "Pickup 4 more cards";
        } else if (value == "King") {
            toolTip = "Gain 3 power";
        } else if (value == "Queen") {
            toolTip = "Power use goes down by 1";
        } else if (value == "Jack") {
            toolTip = "Kill two turn's worth of humans";
        } else if (suit == "Clubs") {
            toolTip = "Kill 1 human";
        } else if (suit == "Hearts") {
            toolTip = "Gain 1 power";
        } else if (suit == "Diamonds") {
            toolTip = "Swarm grows by 1, and power use goes up by 1";
        } else if (suit == "Spades") {
            toolTip = "Gain 1 more turn";
        }
    }

    public void PlayCard() {
        if (controller.canPlayCard()) {
            hand.PlayCard(card_string, true);
            if (value == "Ace") {
                string[] splitRead = controller.lastCard.Split('x');
                SetSuit(splitRead[0]);
                SetValue(splitRead[1]);
                AlertListManager.NewAlert("Your ace became a " + value + " of " + suit, suit);
            }
            if (value == "2") {
                Debug.Log("pick up 2");
                try {
                    for (int i = 0; i < 2; i++) {
                        hand.AddCard(deck.DealRandomCard());
                    }
                } catch (ArgumentOutOfRangeException e) {

                }
            } else if (value == "8" || value == "7") {
                controller.turnsLeft += 2;
                AlertListManager.NewAlert("You gained 2 more turns!", "ui-time");
            } else if (value == "10") {
                try {
                    for (int i = 0; i < 4; i++) {
                        hand.AddCard(deck.DealRandomCard());
                    }
                } catch (ArgumentOutOfRangeException e) {

                }
            } else if (value == "King") {
                controller.power += 3;
                AlertListManager.NewAlert("Your power increased by 3!", "ui-power");
            } else if (value == "Queen") {
                controller.powerUse -= 1;
                AlertListManager.NewAlert("Your power use decreased by 1!", "ui-power");
            } else if (value == "Jack") {
                controller.humans -= controller.nanoSwarms;
                AlertListManager.NewAlert("You killed an extra " + controller.nanoSwarms.ToString() + " humans with your swarm.", "ui-human");
            } else if (suit == "Clubs") {
                controller.humans -= 1;
                AlertListManager.NewAlert("You killed a human!", "ui-human");
            } else if (suit == "Hearts") {
                controller.power += 1;
                AlertListManager.NewAlert("Your power increased by 1!", "ui-power");
            } else if (suit == "Diamonds") {
                controller.nanoSwarms += 1;
                controller.powerUse += 1;
                AlertListManager.NewAlert("Your swarm grew by 1, but so did your power use!", "ui-human");
            } else if (suit == "Spades") {
                controller.turnsLeft += 1;
                AlertListManager.NewAlert("You got an extra turn!", "ui-time");
            }

            hand.soundPlay.Play();
            controller.lastCard = card_string;
            controller.lastCardSpecial = specialGraphic;
            controller.lastCardTooltip = toolTip;
            controller.dirty = true;
            Destroy(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        controller.currentTooltip = toolTip;
        controller.hoverObject = gameObject;
        controller.currentMousePos = eventData.position;
        controller.ttDirty = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        controller.hoverObject = null;
        controller.currentTooltip = null;
        controller.ttDirty = true;
    }
}
