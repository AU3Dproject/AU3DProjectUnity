using UnityEngine;
using System.Collections;

public class AdjustPanelMove : MonoBehaviour {

	RectTransform rectTransform = null;
	bool isAction = false;
	bool isClose = false;

	[SerializeField]
	float closeSpeed = 40.0f;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetButtonDown ("Cancel")) {
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
