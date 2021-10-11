using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class AlertBehaviour : MonoBehaviour, IPointerClickHandler {

    // Use this for initialization
    void Start() {
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameControl>(); 
    }

    string alertText;
    string imageFile;
    GameControl controller;

    // Update is called once per frame
    void Update() {

    }

    public void SetImage(string image) {
        imageFile = image;
        Sprite sprite = Resources.Load<Sprite>(image);
        Transform iconContainer = this.transform.GetChild(0);
        if (sprite != null)
        {
            iconContainer.GetComponent<Image>().sprite = sprite;
        } else {
            iconContainer.gameObject.SetActive(false);
        }
    }

    public void SetText(string text) {
        alertText = text;

        Text textContainer = this.transform.GetComponentInChildren<Text>();
        textContainer.text = alertText;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.pointerId == -2) {
            Delete();
        }
    }

    public void Delete() {
        Destroy(gameObject);
    }
}
