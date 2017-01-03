using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovePlaceDecideButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Button>().onClick.AddListener(SelectButton);
		;
	}

	void SelectButton() {
		Toggle[] contents = GameObject.Find("ScrollPanel").transform.GetChild(0).GetChild(0).GetChild(0).GetComponentsInChildren<Toggle>();

		foreach (Toggle btn in contents) {
			if (btn.GetComponent<Image>().color == Color.yellow) {
				btn.Select();
			}
		}
		contents[0].Select();
	}
}
