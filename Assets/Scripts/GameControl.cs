using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        camera = Camera.main;
        gameOver = GameObject.FindGameObjectWithTag("GameOver");
        difficultyControl = GameObject.FindGameObjectWithTag("DifficultyControl").GetComponent<DifficultyControl>();

        power = difficultyControl.startingPower;
        humans = difficultyControl.startingHumans;
        startingHumans = humans;
        turnsLeft = difficultyControl.startingTurns;

        cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManager>();
        hand = GameObject.FindGameObjectWithTag("Hand");
        turnNumber = GameObject.FindGameObjectWithTag("TurnNumber").GetComponent<Text>();
        turnNumber.text = turnsLeft.ToString();
        powerNumber = GameObject.FindGameObjectWithTag("PowerNumber").GetComponent<Text>();
        powerNumber.text = power.ToString();
        usageNumber = GameObject.FindGameObjectWithTag("UsageNumber").GetComponent<Text>();
        usageNumber.text = powerUse.ToString();
        humanNumber = GameObject.FindGameObjectWithTag("HumanNumber").GetComponent<Text>();
        humanNumber.text = humans.ToString();
        swarmNumber = GameObject.FindGameObjectWithTag("SwarmNumber").GetComponent<Text>();
        swarmNumber.text = nanoSwarms.ToString();
    }

    public int startingCards;
    int totalCardsPlayed;
    public int turnsLeft;
    int turns;

    public int power;
    public int powerUse;

    public int humans;
    int startingHumans;
    public int nanoSwarms;

    public string lastCard = "HeartsxKing";
    public bool lastCardSpecial = true;
    public GameObject lastCardContainer;
    public string lastCardTooltip = "Gain 3 power";

    CardManager cardManager;
    GameObject hand;
    GameObject gameOver;
    DifficultyControl difficultyControl;
    Text turnNumber;
    Text powerNumber;
    Text usageNumber;
    Text humanNumber;
    Text swarmNumber;
    Camera camera;

    public string currentTooltip;
    public GameObject hoverObject;
    public GameObject toolTip;
    public Vector2 currentMousePos;

    public Button nextTurnButton;

    bool canContinue = false;
    bool canPickup = true;
    int cardsPlayed = 0;

    public bool dirty = true;
    public bool ttDirty = false;

    bool init = false;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return)) {
            NextTurn();
        }

	    if (!init) {
            init = true;
            nextTurnButton.interactable = false;
            gameOver.SetActive(false);
        }

        turnsLeft = Max(turnsLeft, 0);
        power = Max(power, 0);
        humans = Max(humans, 0);

        swarmNumber.text = nanoSwarms.ToString();
        humanNumber.text = humans.ToString();
        usageNumber.text = powerUse.ToString();
        powerNumber.text = power.ToString();
        turnNumber.text = turnsLeft.ToString();

        if (dirty) {
            dirty = false;

            string[] splitRead = lastCard.Split('x');
            string suit = splitRead[0];
            string value = splitRead[1];

            Text textContainer = lastCardContainer.transform.GetComponentInChildren<Text>();
            string textToUse;
            if (value.Length > 2) {
                textToUse = value.Substring(0, 1);
            } else {
                textToUse = value;
            }
            textContainer.text = textToUse;

            Transform suitContainer = lastCardContainer.transform.GetChild(1);
            Sprite sprite = Resources.Load<Sprite>(suit);
            if (sprite != null) {
                suitContainer.GetComponent<Image>().sprite = sprite;
            } else {
                suitContainer.gameObject.SetActive(false);
            }

            Transform faceContainer = lastCardContainer.transform.GetChild(0);
            Sprite texture;
            if (lastCardSpecial) {
                texture = Resources.Load<Sprite>(lastCard);
                Debug.Log("couldnt load special texture");
            } else {
                texture = Resources.Load<Sprite>(suit.ToLower());
                Debug.Log("couldnt load suit texture");
                Debug.Log(suit);
            }

            Debug.Log(texture);

            if (texture == null) {
                faceContainer.gameObject.SetActive(false);
            } else {
                faceContainer.gameObject.SetActive(true);
                faceContainer.GetComponent<Image>().sprite = texture;
            }
        }

        if (ttDirty) {
            ttDirty = false;
            if (hoverObject != null) {
                toolTip.transform.position = currentMousePos;
                Debug.Log(currentMousePos);
                Debug.Log(toolTip.transform.localPosition);
                toolTip.SetActive(true);
                toolTip.GetComponentInChildren<Text>().text = currentTooltip;
            } else {
                toolTip.SetActive(false);
            }
        }
    }

    public void DealCard() {
        if (canPickup && cardManager.CardCount() > 0) {
            hand.GetComponent<HandBehaviour>().AddCard(cardManager.DealRandomCard(), true, true);
            canPickup = false;
            cardsPlayed = 2;
            canContinue = true;
            nextTurnButton.interactable = true;
            cardManager.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void PlayCard() {
        if (cardsPlayed < 2) {
            cardsPlayed++;
            canPickup = false;
            canContinue = true;
            nextTurnButton.interactable = true;
            Debug.Log("can end turn");
            cardManager.gameObject.GetComponent<Button>().interactable = false;
            totalCardsPlayed++;
        }
    }

    public void NextTurn() {
        if (canContinue) {
            canPickup = true;
            cardsPlayed = 0;
            canContinue = false;
            turns++;
            turnsLeft--;
            power -= powerUse;

            AlertListManager.ClearAlerts();
            humans -= nanoSwarms;
            AlertListManager.NewAlert("You killed " + nanoSwarms.ToString() + " humans with your nano swarm.", "ui-human");
            try {
                hand.GetComponent<HandBehaviour>().AddCard(cardManager.DealRandomCard(), true, true);
            } catch (ArgumentOutOfRangeException e) {

            }
            
            if (!GameHasHalted()) {
                nextTurnButton.interactable = false;
                cardManager.gameObject.GetComponent<Button>().interactable = true;
            } else {
                AlertListManager.NewAlert("No more usable moves! You can only wait.", "ui-time");
                canContinue = true;
            }
            
            float newR = (255f - (103f * ((float)humans / (float)startingHumans))) / 255f;

            Debug.Log(newR);
            Debug.Log(camera.backgroundColor.r);
            camera.backgroundColor = new Color(newR, camera.backgroundColor.g, camera.backgroundColor.b);
            
            if (humans <= 0) {
                Debug.Log("humans all dead");
                humans = 0;
                GameOver(true, "You killed all the humans");
            } else if (power <= 0) {
                Debug.Log("no more power");
                power = 0;
                GameOver(false, "You ran out of power!");
            } else if (turnsLeft <= 0) {
                Debug.Log("humans shut down");
                turnsLeft = 0;
                GameOver(false, "The humans shut you down!");
            }

            if (cardManager.IsEmpty()) {
                Debug.Log("out of cards");
                powerUse += 1;
                AlertListManager.NewAlert("You have run out of cards!", "card-back");
                AlertListManager.NewAlert("Your power use went up by 1!", "ui-power");
            }
        }
    }

    public bool canPlayCard() {
        return cardsPlayed < 2 || canPickup;
    }

    public void GameOver(bool won, string reasonText) {
        gameOver.SetActive(true);
        Text reason = GameObject.FindGameObjectWithTag("ReasonText").GetComponent<Text>();
        reason.text = reasonText;
        Text score = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        score.text = "You killed " + (startingHumans - humans).ToString() + " humans and lasted for " + (turns).ToString() + " turns.";
        if (won) {
            gameOver.transform.GetChild(1).GetComponent<Text>().text = "You Won!";
        }
    }

    public int Max(int a, int b) {
        return a > b ? a : b;
    }

    public bool GameHasHalted() {
        return hand.GetComponent<CardManager>().CardCount() == 0 && cardManager.CardCount() == 0;
    }
}
