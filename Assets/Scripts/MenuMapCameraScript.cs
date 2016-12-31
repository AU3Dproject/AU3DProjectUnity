using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuMapCameraScript : MonoBehaviour {

	BoxCollider collider = null;
	Transform mapManager;
	Transform movePanel;

	// Use this for initialization
	void Start() {
		collider = GetComponent<BoxCollider>();
		mapManager = GameObject.Find("MapManager").transform;
		movePanel = GameObject.Find("movePanel").transform;
	}

	// Update is called once per frame
	void Update() {

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == 11) {
			other.gameObject.GetComponent<Renderer>().material.color = Color.blue;
			buttonSelect(other, Color.yellow);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.layer == 11) {
			other.gameObject.GetComponent<Renderer>().material.color = Color.red;
			buttonSelect(other,Color.white);
		}
	}

	void buttonSelect(Collider other ,Color color) {
		int index = -1;
		for (int i = 0; i < mapManager.childCount; i++) {
			if (mapManager.GetChild(i) == other.transform.parent) {
				index = i;
				break;
			}
		}
		if (index != -1) {
			GameObject nav_content = GameObject.Find("ScrollPanel").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(index).gameObject;
			nav_content.GetComponent<Image>().color = color;
		}
	}
}
