using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Fungus;

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
		module = GameObject.Find ("EventSystem").GetComponent<StandaloneInputModule>();
		module.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetButtonDown ("Cancel")) {
			flowchart.SetBooleanVariable("StopOther",!flowchart.GetBooleanVariable("StopOther"));
			module.gameObject.SetActive(!module.gameObject.activeSelf);
			isAction = true;
		}
		if (isAction) {
			if(isClose){
				if(rectTransform.anchoredPosition3D.x<150){
					rectTransform.anchoredPosition3D += new Vector3(closeSpeed,0,0);
				}else {
					rectTransform.anchoredPosition3D = new Vector3(150,0,0);
					isClose = false;
					isAction = false;
				}
			}else{
				if(rectTransform.anchoredPosition3D.x>-150){
					rectTransform.anchoredPosition3D -= new Vector3(closeSpeed,0,0);
				}else {
					rectTransform.anchoredPosition3D = new Vector3(-150,0,0);
					isClose = true;
					isAction = false;
				}
			}
		}
	}


}
