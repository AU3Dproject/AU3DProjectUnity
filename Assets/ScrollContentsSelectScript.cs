using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollContentsSelectScript : MonoBehaviour {

	public Button beforeButton;
	private bool isSettingEnd = false;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (isSettingEnd == false) {
			SettingContentSelectListener();
		}
	}

	public void SettingContentSelectListener() {
		for (int i = 0; i < transform.childCount; i++) {
			Button btn = transform.GetChild(i).GetComponent<Button>();
			btn.onClick.AddListener(()=>onclickListener(btn));
		}
	}

	void onclickListener(Button btn) {
		otherColorInit();
		btn.GetComponent<Image>().color = new Color(165.0f/255.0f,255.0f/255.0f,173.0f/255.0f);
		beforeButton.Select();
	}

	void otherColorInit() {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).GetComponent<Image>().color = Color.white;
		}
	}

}
