using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimeTextScript : MonoBehaviour {

    private Text timeText = null;
    private float time;
    private string coron;
    private string secondBuf = ":";

	// Use this for initialization
	void Start () {
        timeText = GetComponent<Text>();
	}

    // Update is called once per frame
    void Update() {
        System.DateTime t = System.DateTime.Now;
        time += Time.deltaTime;

        if(secondBuf != t.Second.ToString()) {
            if (coron == ":") coron = " ";
            else coron = ":";
        }

        timeText.text = String.Format("{0:0000}" + "/" + "{1:00}" + "/" + "{2:00}" + " " + dowEnglish(t.DayOfWeek.ToString()) + " " + "{3:00}" + coron + "{4:00}" + coron + "{5:00}", t.Year,t.Month,t.Day,t.Hour, t.Minute, t.Second);
        secondBuf = t.Second.ToString();
    }

    string dowKanji(string dow) {
        switch (dow) {
            case "Sunday":
                return "日";
            case "Monday":
                return "月";
            case "Tuesday":
                return "火";
            case "Wednesday":
                return "水";
            case "Thursday":
                return "木";
            case "Friday":
                return "金";
            case "Saturday":
                return "土";
        }
        return "null";
    }

    string dowEnglish(string dow) {
        switch (dow) {
            case "Sunday":
                return "Sun";
            case "Monday":
                return "Mon";
            case "Tuesday":
                return "Tue";
            case "Wednesday":
                return "Wed";
            case "Thursday":
                return "Thu";
            case "Friday":
                return "Fri";
            case "Saturday":
                return "Sat";
        }
        return "null";
    }
}
