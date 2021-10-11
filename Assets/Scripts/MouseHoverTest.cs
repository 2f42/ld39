using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseHoverTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	// Use this for initialization
	void Start () {
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameControl>();
	}

    GameControl controller;

	// Update is called once per frame
	void Update () {
	    
	}

    public void OnPointerEnter(PointerEventData e) {
        controller.currentTooltip = controller.lastCardTooltip;
        controller.hoverObject = gameObject;
        controller.currentMousePos = e.position;
        controller.ttDirty = true;
    }

    public void OnPointerExit(PointerEventData e) {
        controller.hoverObject = null;
        controller.currentTooltip = null;
        controller.ttDirty = true;
    }
}
