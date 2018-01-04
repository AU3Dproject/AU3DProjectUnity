using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : ManagerMonoBehaviour<TalkManager>{

	[SerializeField]
	[Header("Required - ここは設定してね")]
	public GameObject talk_canvas = null;
	[Space(10),Header("Optional - TalkCanvasの構造に変化がなければ未指定で良い")]
	public GameObject text_window = null;
	public GameObject select_panel = null;

	public GameObject getTalkCanvas() {
		return talk_canvas;
	}

	public GameObject getTextWindow() {
		if(text_window == null) {
			return talk_canvas.transform.Find("TextWindow").gameObject;
		} else {
			return text_window;
		}
		
	}

	public Writer getWriter() {
		if (text_window == null) {
			return talk_canvas.transform.Find("TextWindow").gameObject.GetComponent<Writer>();
		} else {
			return text_window.GetComponent<Writer>();
		}

	}

	public GameObject getSelectPanel() {
		if(select_panel == null) {
			return talk_canvas.transform.Find("SelectPanel").gameObject;
		} else {
			return select_panel;
		}
		
	}

	public TalkEventSelectButton getSelectButton() {
		if(select_panel == null) {
			return talk_canvas.transform.Find("SelectPanel").GetComponent<TalkEventSelectButton>();
		} else {
			return select_panel.GetComponent<TalkEventSelectButton>();
		}
		
	}
	
}
