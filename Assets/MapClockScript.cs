using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MapClockScript : MonoBehaviour {

	[SerializeField]
	[Tooltip("マップを時計みたいな感じにするかどうか")]
	public bool mapClockMode = true;
	private RectTransform t;

	private Image imageComponent;
	private virtualTimeScript virtualTime;

	// Use this for initialization
	void Start () {
		if(imageComponent == null)imageComponent = GetComponent<Image>();
		virtualTime = GameObject.FindGameObjectWithTag("Time").GetComponent<virtualTimeScript>();
	}
	
	// Update is called once per frame
	void Update () {
		float timeRate = virtualTime.getTimeRateOfDay();

		if (mapClockMode) {

			if (imageComponent != null) {
			
				if (timeRate < 0.5f) {
					imageComponent.fillClockwise = false;
					imageComponent.fillAmount = 1.0f - (timeRate / 0.5f);
				} else {
					imageComponent.fillClockwise = true;
					imageComponent.fillAmount = timeRate * 2.0f - 1.0f;
				}
					

			}

		}

	}
}
