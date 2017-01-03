using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuMapCameraScript : MonoBehaviour {
	
	Transform mapManager;
	Transform movePanel;

	Collider selectedCollider = null;

	// Use this for initialization
	void Start() {
		mapManager = GameObject.Find("MapManager").transform;
		movePanel = GameObject.Find("movePanel").transform;
	}

	// Update is called once per frame
	void Update() {
		if (selectedCollider != null) {
			mapPlaneSelect(selectedCollider, Color.blue);
			buttonSelect(selectedCollider, Color.yellow);
		} else {

		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == 11) {
			selectedCollider = other;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.layer == 11) {
			mapPlaneSelect(other, Color.red);
			buttonSelect(other,Color.white);
			selectedCollider = null;
		}
	}

	void mapPlaneSelect(Collider other, Color color) {
		other.gameObject.GetComponent<Renderer>().material.color = color;
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
