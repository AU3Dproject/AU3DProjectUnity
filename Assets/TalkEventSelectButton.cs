using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TalkEventSelectButton : MonoBehaviour {

	public int Answer = -1;
	public bool isAnswer;

	private GameObject buttonPrefab = null;
	private GameObject[] buttons = new GameObject[8];

	// Use this for initialization
	void Start () {
		buttonPrefab = (GameObject)Resources.Load("TalkEvent/TalkEventButton");
		for (int i = 0; i < buttons.Length; i++) {
			buttons[i] = buttonPrefab;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void setSelectButton(string[] labels) {
		float baseY = -840.0f / labels.Length;
		for (int i = 0; i < labels.Length; i++) {
			buttons[i].transform.GetChild(0).GetComponent<Text>().text = labels[i];
			buttons[i].GetComponent<RectTransform>().position.Set(0, baseY * i, 0);
			buttons[i].name = ""+i;
			buttons[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(()=>setAnswer(buttons[i]));
		}
		Answer = -1;
	}
	public void closeButtons() {
		for (int i = 0; i < buttons.Length; i++) {
			buttons[i].SetActive(false);
		}
	}

	public void setAnswer(int ans) {
		Answer = ans;
		isAnswer = true;
	}
	public void setAnswer(GameObject ans) {
		Answer = int.Parse(ans.name);
		isAnswer = true;
	}
	public int getAnswer() {
		if (isAnswer) {
			return Answer;
		} else {
			return -1;
		}
	}
}
