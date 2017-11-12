using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuContent : MonoBehaviour {

	public GameObject firstSelect = null;
	public string selected_description = "";
	private EventSystem event_system;

	// Use this for initialization
	void Start () {
		event_system = EventSystem.current;
	}

	void Update() {
		//Debug.Log(event_system.currentSelectedGameObject);
	}

	public void SelectFirst() {
		if (firstSelect != null) {

			// if (selected_description != "") {
			// 	MainMenuManager.Instance.description_component.setDescription(selected_description);
			// }
			// if (firstSelect.GetComponentInChildren<EnhancedScroller>() != null) {
			// 	// firstSelect.GetComponentInChildren<EnhancedScroller>().FocusCell();
			// } else {
			// 	event_system.SetSelectedGameObject(null);
			// 	event_system.SetSelectedGameObject(firstSelect);
			// }
		}
	}

}
