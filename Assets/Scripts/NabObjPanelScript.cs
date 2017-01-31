using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class NabObjPanelScript : MonoBehaviour, ISelectHandler,IDeselectHandler,ISubmitHandler {

	[SerializeField]
	public ButtonState state = ButtonState.Normal;
	[SerializeField]
	public Color[] stateColor = {
		Color.white,Color.yellow,Color.green,Color.green+Color.yellow
	};
	[SerializeField]
	public enum ButtonState {
		Normal,
		Pressed,
		Highlighted,
		Hilighted_and_Pressed
	};
	private Image image;
	private Button button;

	// Use this for initialization
	void Start() {
		image = GetComponent<Image>();
		stateColor[(int)ButtonState.Normal] = Color.white;
		stateColor[(int)ButtonState.Pressed] = Color.yellow;
		stateColor[(int)ButtonState.Highlighted] = Color.green;
	}

	// Update is called once per frame
	void Update() {
		image.color = stateColor[(int)state];
	}

	public void changeState(ButtonState state) {
		this.state = state;
	}

	public bool isState(ButtonState state) {
		return (this.state == state);
	}

	public void OnSelect(BaseEventData eventData) {
		changeState(ButtonState.Highlighted);
	}

	public void OnDeselect(BaseEventData eventData) {
		changeState(ButtonState.Normal);
	}

	public void OnSubmit(BaseEventData eventData) {
		changeState(ButtonState.Pressed);
	}
}
