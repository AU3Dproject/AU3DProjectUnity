using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimeTextScript : MonoBehaviour {

    private Text timeText = null;

	// Use this for initialization
	void Start () {
        timeText = GetComponent<Text>();
	}

    // Update is called once per frame
    void Update() {
        System.DateTime t = System.DateTime.Now;
        timeText.text = t.Year + "年" + t.Month + "月" + t.Day + "日" + "(" + dow(t.DayOfWeek.ToString()) + ")" + t.Hour + "時" + t.Minute + "分" + t.Second + "秒";
    }

    string dow(string dow) {
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
}
