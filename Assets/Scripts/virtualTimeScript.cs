using UnityEngine;
using System.Collections;
using System;

public class virtualTimeScript : MonoBehaviour {

	[SerializeField]
	public DateTime virtualDateTime;

	[Header("リアルタイムモードのTrue/False")]
	[Tooltip("現実の時間を使用するかどうか")]
	public bool realTimeMode = true;

	[Header("リアルタイムモードがFalseの場合の初期の日付時間")]
	[Tooltip("現実時間を使用しない場合の初期の年")]
	public int initYear;
	[Tooltip("現実時間を使用しない場合の初期の月")]
	public int initMonth;
	[Tooltip("現実時間を使用しない場合の初期の日にち")]
	public int initDay;
	[Tooltip("現実時間を使用しない場合の初期の時間")]
	public int initHour;
	[Tooltip("現実時間を使用しない場合の初期の分")]
	public int initMinute;
	[Tooltip("現実時間を使用しない場合の初期の秒")]
	public int initSecond;

	[Header("リアルタイムモードがFalseの場合の一日にかかる秒数")]
	[Tooltip("一日にかかる現実の秒数(例えば86400秒なら現実もゲームも1秒経過するが、半分の43200秒なら現実の0.5秒でゲームは1秒経過する)")]
	public int virtualDaySeconds = 86400;

	[Header("月日の文字列化に関する設定")]
	[Tooltip("年を区切る文字列(年の後ろの文字)")]
	public string yearSeparate = "/";
	[Tooltip("月を区切る文字列(月の後ろの文字)")]
	public string monthSeparate = "/";
	[Tooltip("日を区切る文字列(日の後ろの文字)")]
	public string daySeparate = "";
	[Tooltip("時を区切る文字列(時の後ろの文字)")]
	public string hourSeparate = ":";

	[Tooltip("分を区切る文字列(分の後ろの文字)")]
	public string minuteSeparate = ":";
	[Tooltip("秒を区切る文字列(秒の後ろの文字)")]
	public string secondSeparate = " ";
	[Tooltip("曜日を区切る文字列(曜日の後ろの文字)(dowはday of weekの略)")]
	public string dowSeparate = "曜日 ";

	public int flashingPeriod = 1;

	public enum DowType {
		English,
		omitEnglish,
		Japanese,
	}
	public enum DateTimeStringType {
		Date,Time,DateAndTime,
	}
	public enum HourStringType {
		_24h,_12h,ampm,
	}
	public enum FillStringType {
		Zero,Space
	}

	private float time = 0.0f;
	private float limitSecond = 1.0f;
	private bool flashing = true;
	private float flashingCount = 0;

	// Use this for initialization
	void Start() {
		initVirtualDateTime(realTimeMode);
	}

	// Update is called once per frame
	void Update() {		

		if (realTimeMode) {
			virtualDateTime = DateTime.Now;
		} else {

			time += Time.deltaTime;
			

			limitSecond = virtualDaySeconds / 86400.0f;

			if (time >= limitSecond) {

				if (time > limitSecond * 2.0f) {
					virtualDateTime = virtualDateTime.AddSeconds(time/limitSecond);
					time = 0;
				}else {
					virtualDateTime = virtualDateTime.AddSeconds(1);
					time -= limitSecond;
				}

			}

		}

		if (flashingPeriod > 0) {
			flashingCount += Time.deltaTime;
			if (flashingCount >= flashingPeriod) {
				flashing = !flashing;
				flashingCount -= flashingPeriod;
			}
		} else {
			if (virtualDateTime.Second % 2 == 0) {
				flashing = true;
			}else {
				flashing = false;
			}
		}

	}

	void initVirtualDateTime(bool realTimeMode) {
		if (realTimeMode) virtualDateTime = DateTime.Now;
		else {
			virtualDateTime = new DateTime(
				initYear, initMonth, initDay, initHour, initMinute, initSecond
			);
		}
	}

	public float getTimeRateOfDay() {
		return (virtualDateTime.Hour * 60.0f * 60.0f + virtualDateTime.Minute * 60.0f + virtualDateTime.Second) / (86400.0f);
	}

	public string ToString(bool isTimeFlashing = false ,DowType dowType=DowType.Japanese,DateTimeStringType dateTimeType = DateTimeStringType.DateAndTime,HourStringType hourType = HourStringType._24h,FillStringType fillType = FillStringType.Zero) {
		string year="", month = "", day = "", hour = "", minute = "", second = "", dow = "";
		if (dateTimeType == DateTimeStringType.DateAndTime || dateTimeType == DateTimeStringType.Date) {
			if (fillType == FillStringType.Space) {
				year	= string.Format("{0:4}" + yearSeparate,	virtualDateTime.Year);
				month	= string.Format("{0:2}" + monthSeparate,virtualDateTime.Month);
				day		= string.Format("{0:2}" + daySeparate,	virtualDateTime.Day);
			} else if (fillType == FillStringType.Zero) {
				year	= string.Format("{0:D4}" + yearSeparate,	virtualDateTime.Year);
				month	= string.Format("{0:D2}" + monthSeparate,	virtualDateTime.Month);
				day		= string.Format("{0:D2}" + daySeparate,		virtualDateTime.Day);
			}
			if (dowType == DowType.English) {
				dow = virtualDateTime.DayOfWeek.ToString() + dowSeparate;
			} else if (dowType == DowType.Japanese) {
				dow = dowKanji(virtualDateTime.DayOfWeek.ToString()) + dowSeparate;
			} else if (dowType == DowType.omitEnglish) {
				dow = dowOmitEnglish(virtualDateTime.DayOfWeek.ToString()) + dowSeparate;
			}
		}
		if (dateTimeType == DateTimeStringType.DateAndTime || dateTimeType == DateTimeStringType.Time) {
			if (fillType == FillStringType.Space) {
				hour	= string.Format("{0:2}" + hourSeparate,		virtualDateTime.Hour);
				minute	= string.Format("{0:2}" + minuteSeparate,	virtualDateTime.Minute);
				second	= string.Format("{0:2}" + secondSeparate,	virtualDateTime.Second);
			} else if (fillType == FillStringType.Zero) {
				hour	= string.Format("{0:D2}" + (virtualDateTime.Second % 2 == 0 ? hourSeparate   : " "),	virtualDateTime.Hour);
				minute	= string.Format("{0:D2}" + (virtualDateTime.Second % 2 == 0 ? minuteSeparate : " "),	virtualDateTime.Minute);
				second	= string.Format("{0:D2}" + (virtualDateTime.Second % 2 == 0 ? secondSeparate : " "),	virtualDateTime.Second);
			}
		}
		return (year + month + day + dow + hour + minute + second);
	}

	public string ToStringRich(Color textColor,bool isTimeFlashing = false, DowType dowType = DowType.Japanese, DateTimeStringType dateTimeType = DateTimeStringType.DateAndTime, HourStringType hourType = HourStringType._24h, FillStringType fillType = FillStringType.Zero) {
		string year = "", month = "", day = "", hour = "", minute = "", second = "", dow = "";
		if (dateTimeType == DateTimeStringType.DateAndTime || dateTimeType == DateTimeStringType.Date) {
			if (fillType == FillStringType.Space) {
				year = string.Format("{0:4}" + yearSeparate, virtualDateTime.Year);
				month = string.Format("{0:2}" + monthSeparate, virtualDateTime.Month);
				day = string.Format("{0:2}" + daySeparate, virtualDateTime.Day);
			} else if (fillType == FillStringType.Zero) {
				year = string.Format("{0:D4}" + yearSeparate, virtualDateTime.Year);
				month = string.Format("{0:D2}" + monthSeparate, virtualDateTime.Month);
				day = string.Format("{0:D2}" + daySeparate, virtualDateTime.Day);
			}
			if (dowType == DowType.English) {
				dow = virtualDateTime.DayOfWeek.ToString() + dowSeparate;
			} else if (dowType == DowType.Japanese) {
				dow = dowKanji(virtualDateTime.DayOfWeek.ToString()) + dowSeparate;
			} else if (dowType == DowType.omitEnglish) {
				dow = dowOmitEnglish(virtualDateTime.DayOfWeek.ToString()) + dowSeparate;
			}
		}
		textColor.a = 0;
		string command = ColorUtility.ToHtmlStringRGBA(textColor);
		if (dateTimeType == DateTimeStringType.DateAndTime || dateTimeType == DateTimeStringType.Time) {
			if (fillType == FillStringType.Space) {
				hour = string.Format("{0:2}" + (isTimeFlashing?flashingSeparate(virtualDateTime.Second, hourSeparate, textColor):hourSeparate), virtualDateTime.Hour);
				minute = string.Format("{0:2}" + (isTimeFlashing ? flashingSeparate(virtualDateTime.Second, minuteSeparate, textColor) : minuteSeparate), virtualDateTime.Minute);
				second = string.Format("{0:2}" + (isTimeFlashing ? flashingSeparate(virtualDateTime.Second, secondSeparate, textColor) : secondSeparate), virtualDateTime.Second);
			} else if (fillType == FillStringType.Zero) {
				hour = string.Format("{0:D2}" + flashingSeparate(virtualDateTime.Second,hourSeparate,textColor), virtualDateTime.Hour);
				minute = string.Format("{0:D2}" + flashingSeparate(virtualDateTime.Second, minuteSeparate, textColor), virtualDateTime.Minute);
				second = string.Format("{0:D2}" + flashingSeparate(virtualDateTime.Second, secondSeparate, textColor), virtualDateTime.Second);
			}
		}
		return (year + month + day + dow + hour + minute + second);
	}

	string flashingSeparate(int value,string separate,Color color) {
		string ret = "";
		if(flashing) {
			color.a = 0;
			ret = string.Format("<color=#{0}>"+separate+"</color>",ColorUtility.ToHtmlStringRGBA(color));
		}else {
			ret = separate;
		}
		return ret;
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

	string dowOmitEnglish(string dow) {
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
