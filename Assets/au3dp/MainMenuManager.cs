using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : ManagerMonoBehaviour<MainMenuManager> {

	[SerializeField]
	public MenuDescription title_component;
	public MenuDescription description_component;
	public MainMenuContentsManager contents_manager_component;
	public MainMenuButtonGroup main_menu_button_group_component;
	private bool isOpen = false;

	// Use this for initialization
	void Start () {
		Close();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel") && main_menu_button_group_component.isInteractive) {
			SwitchingOpen();
		}
	}

	public void SwitchingOpen() {
		if (isOpen) {
			Close();
			PlayerManager.Instance.is_pause = false;
		} else {
			Open();
			PlayerManager.Instance.is_pause = true;
		}
	}

	public void Open() {
		isOpen = true;
		foreach (Transform menu in transform) {
			menu.gameObject.SetActive(true);
		}
		PlayerManager.Instance.is_pause = true;
	}

	public void Close() {
		isOpen = false;
		foreach (Transform menu in transform) {
			menu.gameObject.SetActive(false);
		}
		PlayerManager.Instance.is_pause = false;
		main_menu_button_group_component.isInteractive = true;
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void SetDescription(string description) {
		if (description_component != null) {
			description_component.setDescription(description);
		}
	}

	public void SetTitle(string title) {
		if (title_component != null) {
			title_component.setDescription(title);
		}
	}

	public void FocusContents(int index) {
		if (contents_manager_component != null) {
			contents_manager_component.FocusContents(index);
		}
	}

	 

}
