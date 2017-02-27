using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MainMenuButton : Toggle {

	public MainMenuButtonInformation information = null;
	public MainMenuContent content = null;
	private IconRotation rotative_icon;

	protected override void Start() {
		if (information == null) {
			information = GetComponent<MainMenuButtonInformation>();
		}
		if (rotative_icon == null) {
			rotative_icon = GetComponentInChildren<IconRotation>();
		}
		if (content == null) {
			for (int i = 0; i < transform.parent.childCount; i++) {
				Debug.Log(i);
				if (transform.parent.GetChild(i) == transform) {
					content = MainMenuManager.Instance.contents_manager_component.transform.GetChild(0).GetChild(i).GetComponent<MainMenuContent>();
					break;
				}
			}
		}
	}

	public void SetIconRotation(bool is_icon_rot) {
		if (rotative_icon != null) {
			rotative_icon.setRotate(is_icon_rot);
		}
	}

	void Update() {
		if (content != null) {
			if (isOn && Input.GetButtonDown("Submit")) {
				content.SelectFirst();
			}
		}
	}

}
