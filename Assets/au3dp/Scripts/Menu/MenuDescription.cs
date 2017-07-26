using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDescription : MonoBehaviour {
	
	private Text text_component = null;

	// Use this for initialization
	void Start () {
		if (text_component == null) {
			if ((text_component = transform.GetChild(0).GetComponent<Text>()) == null) {
				throw new UnityException("DescriptionオブジェクトにTextComponentの子がいません。");
			}
		}

	}

	public void setDescription(string description) {
		text_component.text = description;
	}
}
