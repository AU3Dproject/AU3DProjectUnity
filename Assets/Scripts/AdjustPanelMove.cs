using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Fungus;
using UnityEngine.UI;

public class AdjustPanelMove : MonoBehaviour {

	RectTransform rectTransform = null;
	bool isClose = false;

	[SerializeField]
	float closeSpeed = 40.0f;
	bool isAction = false;

	private Flowchart flowchart;
	private StandaloneInputModule module ;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
		flowchart = GameObject.Find ("Flowchart").GetComponent<Flowchart>();
		DetachChild ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetButtonDown ("Cancel") && (flowchart.GetBooleanVariable("StopOther")==false || isClose==true)) {
			flowchart.SetBooleanVariable("StopOther",!isClose);
			isAction = true;
		}
		if (isAction) {
			if (isClose) {
				if (rectTransform.anchoredPosition3D.x < 150) {
					rectTransform.anchoredPosition3D += new Vector3 (closeSpeed, 0, 0);
				} else {
					rectTransform.anchoredPosition3D = new Vector3 (150, 0, 0);
					isClose = false;
					isAction = false;
					DetachChild ();
				}
			} else {
				if (rectTransform.anchoredPosition3D.x > -150) {
					rectTransform.anchoredPosition3D -= new Vector3 (closeSpeed, 0, 0);
				} else {
					rectTransform.anchoredPosition3D = new Vector3 (-150, 0, 0);
					isClose = true;
					isAction = false;
					AttachChild ();
				}
			}
		} else if(!flowchart.GetBooleanVariable("StopOther")) {
			if(!isClose)DetachChild ();
		}
	}

	void AttachChild(){
		foreach(Transform child in transform){
			if(child.gameObject.tag=="FirstSelect"){
				(child.gameObject.GetComponent<Slider>()).Select ();
				break;
			}
		}
	}
	void DetachChild(){
		EventSystem.current.SetSelectedGameObject (null);
	}


}
