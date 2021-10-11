using UnityEngine;
using System.Collections;

public class AlertListManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        _instance = this;

	    if (alertList == null) {
            alertList = this.gameObject;
        }
        if (alerts == null) {
            alerts = new ArrayList();
        }
	}

    private static AlertListManager _instance;
    public static AlertListManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<AlertListManager>();
            }

            return _instance;
        }
    }

    public GameObject alertPrefab;
    public GameObject alertList;
    ArrayList alerts;
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void NewAlert(string alertText, string alertImage) {
        GameObject newAlert = Instantiate(_instance.alertPrefab);

        newAlert.GetComponent<AlertBehaviour>().SetImage(alertImage);
        newAlert.GetComponent<AlertBehaviour>().SetText(alertText);

        newAlert.transform.SetParent(_instance.alertList.transform);
        newAlert.transform.localScale = Vector3.one;

        _instance.alerts.Add(newAlert);
    }

    public static void ClearAlerts() {
        foreach (GameObject alert in _instance.alerts) {
            alert.GetComponent<AlertBehaviour>().Delete();
        }
        _instance.alerts.Clear();
    }
}
