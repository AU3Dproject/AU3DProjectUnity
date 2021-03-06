﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtonGroup : ToggleGroup{

	private MainMenuButton[] toggles;
	private int on_index = 0;
	public bool isInteractive = true;

	protected override void Start() {
		on_index = 0;
		toggles = GetComponentsInChildren<MainMenuButton>();
		toggles[on_index].isOn = true;
	}

	// Update is called once per frame
	void Update () {
		if (isInteractive) {
			if (Input.GetButtonDown("MoveHorizontal")) {
				if (Input.GetAxisRaw("MoveHorizontal") < 0) {
					BeforeOnToggle();
				} else if (Input.GetAxisRaw("MoveHorizontal") > 0) {
					NextOnToggle();
				}
			}
			if (Input.GetButtonDown("L1")) {
				BeforeOnToggle();
			}
			if (Input.GetButtonDown("R1")) {
				NextOnToggle();
			}
		} else {
			if (Input.GetButtonDown("Cancel")) {
				isInteractive = true;
				EventSystem.current.SetSelectedGameObject(null);
			}
		}
	}

	public void SetOnToggle(int index) {
		for (int i = 0; i < toggles.Length; i++) {
			if (i == index) {
				toggles[i].isOn = true;
				toggles[i].SetIconRotation(true);
				MainMenuManager.Instance.SetDescription(toggles[i].information.description);
				MainMenuManager.Instance.SetTitle(toggles[i].information.title);
				MainMenuManager.Instance.FocusContents(i);
			} else {
				toggles[i].isOn = false;
				toggles[i].SetIconRotation(false);
			}
		}
	}

	public void NextOnToggle() {
		//toggles[on_index].isOn = false;
		//toggles[on_index].SetIconRotation(false);
		on_index = (on_index + 1) % toggles.Length;
		//toggles[on_index].isOn = true;
		//toggles[on_index].SetIconRotation(true);
		//MainMenuManager.Instance.SetDescription(toggles[on_index].information.description);
		//MainMenuManager.Instance.SetTitle(toggles[on_index].information.title);
		//MainMenuManager.Instance.FocusContents(on_index);
		SetOnToggle(on_index);
	}

	public void BeforeOnToggle() {
		//toggles[on_index].isOn = false;
		//toggles[on_index].SetIconRotation(false);
		on_index = (on_index + toggles.Length - 1) % toggles.Length;
		//toggles[on_index].isOn = true;
		//toggles[on_index].SetIconRotation(true);
		//MainMenuManager.Instance.SetDescription(toggles[on_index].information.description);
		//MainMenuManager.Instance.SetTitle(toggles[on_index].information.title);
		//MainMenuManager.Instance.FocusContents(on_index);
		SetOnToggle(on_index);
	}

	public void OnToggleUpdate() {
		for (int i = 0; i < toggles.Length; i++) {
			if (toggles[i].isOn) {
				on_index = i;
			}
		}
		SetOnToggle(on_index);
	}
}
