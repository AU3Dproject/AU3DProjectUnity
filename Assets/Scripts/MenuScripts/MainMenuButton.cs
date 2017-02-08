using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MainMenuButton : Toggle {

	public MainMenuButtonInformation information = null;
	private IconRotation rotative_icon;

	protected override void Start() {
		if (information == null) {
			information = GetComponent<MainMenuButtonInformation>();
		}
		if (rotative_icon == null) {
			rotative_icon = GetComponentInChildren<IconRotation>();
		}
	}

	public void SetIconRotation(bool is_icon_rot) {
		rotative_icon.setRotate(is_icon_rot);
	}

}
