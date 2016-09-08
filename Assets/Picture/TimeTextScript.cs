using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeTextScript : MonoBehaviour {

    private Text timeText = null;

	// Use this for initialization
	void Start () {
        timeText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        timeText.text = System.DateTime.Now.ToString();
	}
}
