using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MainMenuButton : Toggle {

	public MainMenuButtonInformation information = null;
	public MainMenuContent content = null;
	private IconRotation rotative_icon;

	protected override void Awake() {
		if (information == null) {
			information = GetComponent<MainMenuButtonInformation>();
		}
		if (rotative_icon == null) {
			rotative_icon = GetComponentInChildren<IconRotation>();
		}
		if (content == null) {
			for (int i = 0; i < transform.parent.childCount; i++) {
				if (transform.parent.GetChild(i) == transform) {
					content = MainMenuManager.Instance.contents_manager_component.transform.GetComponentsInChildren<MainMenuContent>()[i];
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

	public void Update() {
		if (content != null) {
			if (isOn && Input.GetButtonDown("Submit") && (group as MainMenuButtonGroup).isInteractive) {
				content.SelectFirst();
				(group as MainMenuButtonGroup).isInteractive = false;
			}
		}

		if ((group as MainMenuButtonGroup).isInteractive) {
			var color = graphic.color;
			color.a = 0.5f;
			graphic.color = color;
		} else {
			var color = graphic.color;
			color.a = 0;
			graphic.color = color;
		}
	}

}
