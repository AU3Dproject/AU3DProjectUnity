using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TalkEventSelectButton : MonoBehaviour {

	public string answerLabel = "";
	public bool isAnswer;

	private GameObject buttonPrefab = null;
	private List<GameObject> buttons = new List<GameObject>();
	private string[] labels = new string[8];
	private string[] names = new string[8];
	private int noButton = 0;

	// Use this for initialization
	void Start () {
		buttonPrefab = (GameObject)Resources.Load("TalkEvent/TalkEventButton");
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void openButtons() {
		float baseY = -900.0f / (noButton+1);
		for (int i = 0; i < noButton; i++) {
			GameObject instant = Instantiate(buttonPrefab, transform) as GameObject;
			instant.name = labels[i];
			instant.transform.GetChild(0).GetComponent<Text>().text = names[i];
			instant.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, baseY * (i+1));
			instant.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			instant.GetComponent<Button>().onClick.AddListener(() => setAnswer(instant));
			buttons.Add(instant);
		}
		if(buttons[0].activeInHierarchy) EventSystem.current.SetSelectedGameObject(buttons[0]);
		for (int i = 0; i < noButton; i++) {
			Navigation nav = buttons [i].GetComponent<Button> ().navigation;
			nav.mode = Navigation.Mode.Explicit;
			if (i - 1 >= 0) {
				nav.selectOnUp = buttons [i - 1].GetComponent<Button> ();
			}else {
				nav.selectOnUp = buttons [noButton-1].GetComponent<Button> ();
			}
			if (i + 1 < noButton) {
				nav.selectOnDown = buttons [i + 1].GetComponent<Button> ();
			} else {
				nav.selectOnDown = buttons [0].GetComponent<Button> ();
			}
			nav.selectOnLeft = null;
			nav.selectOnRight = null;
			buttons [i].GetComponent<Button> ().navigation = nav;
		}
	}
	public void closeButtons() {
		for (int i = 0; i < noButton; i++) {
			Destroy(buttons[i]);
		}
		buttons = new List<GameObject>();
		answerLabel = "";
		noButton = 0;
	}

	public void addButton(string name,string label) {
		names[noButton] = name;
		labels[noButton] = label;
		noButton++;
	}

	public void setAnswer(string ans) {
		answerLabel = ans;
		isAnswer = true;
	}
	public void setAnswer(GameObject ans) {
		answerLabel = ans.name;
		isAnswer = true;
		EventSystem.current.SetSelectedGameObject(null);
	}
	public string getAnswer() {
		if (isAnswer) {
			return answerLabel;
		} else {
			return "";
		}
	}
}
