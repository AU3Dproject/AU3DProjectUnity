using UnityEngine;
using System.Collections;
using System;

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
	GameObject canvas;
	private float time = 2.0f;
	int m = 0;

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
		canvas = Instantiate(textWindow, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		stringReader = new System.IO.StringReader(eventScript);
	}

	// Update is called once per frame
	void Update() {
		//stringReader = new System.IO.StringReader(eventScript);

		time -= Time.deltaTime;

		if (time < 0) {
			if (m == 0) {
				Execute();
				Debug.Log(time);
			}
			m = 1;
		} else {
		}
	}

	void Execute() {
		while (stringReader.Peek() > -1) {
			string line = stringReader.ReadLine();
			Debug.Log(line);
			if (isFunction(line)) {
				functionExecute();
			} else if (line == "[p]") {
				canvas.transform.GetChild(0).GetComponent<Writer>().isTextActive = true;
			} else if (line == "[l]") {
				canvas.transform.GetChild(0).GetComponent<Writer>().text = "";
			} else {
				canvas.transform.GetChild(0).GetComponent<Writer>().text += (line+"\n");
			}
		}
	}

	void functionExecute() {

	}

	bool isFunction(string target) {
		return false;
	}
}

