using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TalkEventScript : MonoBehaviour {

	[SerializeField]
	public int page;
	[Tooltip("実行のタイプ\nattach:対象オブジェクトに話しかけた際に実行\nauto:条件下で一度だけ自動実行\nparallel:条件下で永遠に自動実行")]
	public ExecuteType executeType = ExecuteType.attach;
	public string description;
	public enum ExecuteType {
		attach, auto, parallel,
	}
	public GameObject textWindow;
	private System.IO.StringReader stringReader;
	GameObject canvas = null;
	private float time = 2.0f;
	int m = 0;

	private bool isWait = false;
	public bool isExecute = false;

	private List<string> scriptLines = new List<string>();

	[TextArea(10, 10000)]
	[Tooltip(
		"イベント本文です。\n" +
		"command\n" +
		"[p] : 入力待ち\n" +
		"[l] : 改ページ"
	)]
	public string eventScript = "";

	// Use this for initialization
	void Start() {
		stringReader = new System.IO.StringReader(eventScript);
	}

	// Update is called once per frame
	void Update() {
		//stringReader = new System.IO.StringReader(eventScript);

		if (isExecute) {
			Execute();
		}

	}

	void Execute() {

		if (canvas == null) {
			canvas = Instantiate(textWindow, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		}

		if (isWait) {
			if (Input.GetButtonDown("Submit")) {
				isWait = false;
			}
		}
		while (isWait == false && stringReader.Peek() > -1) {
			string line = stringReader.ReadLine();
			Debug.Log(line);
			if (isFunction(line)) {
				functionExecute();
			} else if (line == "[p]") {
				Debug.Log("p:"+line);
				isWait = true;
			} else if (line == "[l]") {
				Debug.Log("l:"+line);
				canvas.transform.GetChild(0).GetComponent<Writer>().text = "";
				canvas.transform.GetChild(0).GetComponent<Writer>().removeText();
			} else {
				Debug.Log("view:"+line);
				canvas.transform.GetChild(0).GetComponent<Writer>().text += (line+"\n");
				canvas.transform.GetChild(0).GetComponent<Writer>().isTextActive = true;
			}
			
		}
		if (isWait == false && stringReader.Peek() <= -1 && canvas.transform.GetChild(0).GetComponent<Writer>().isTextActive == false) {
			GameObject.Destroy(canvas);
		}
	}

	void functionExecute() {

	}

	bool isFunction(string target) {
		return false;
	}

	public void setWait(bool wait) {
		isWait = wait;
	}
}

