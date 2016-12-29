using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour {

	public RectTransform detailContent;

	public List<NavigationDetail> details;

	public GameObject detailPrefab;

	public GameObject NavigationAgent;

	public Material selectMaterial;
	public Material nonSelectMaterial;

	// Use this for initialization
	void Start() {
		detailContent.sizeDelta = new Vector2(0, transform.childCount * 200);
		for (int i = 0; i < transform.childCount; i++) {
			NavigationDetail detail = transform.GetChild(i).GetComponent<NavigationDetail>();
			details.Add(detail);
			GameObject clone = (GameObject)Instantiate(detailPrefab, detailContent);
			clone.name = i.ToString();

			RectTransform t = clone.GetComponent<RectTransform>();
			t.localPosition = new Vector3(0, i * -200, 0);
			t.sizeDelta = new Vector2(900, 200);
			t.localScale = new Vector3(1, 1, 1);

			t.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(detail.ThumbnailAddr);
			t.GetChild(1).GetComponent<Text>().text = detail.NavigationName;
			t.GetChild(2).GetChild(0).GetComponent<Text>().text = detail.NavigationDescription;

			t.GetComponent<Button>().onClick.AddListener(() => setDestination(detail.gameObject));

		}

		for (int i = 0; i < detailContent.childCount; i++) {
			Button button = detailContent.GetChild(i).GetComponent<Button>();
			Navigation button_nav = button.navigation;
			button_nav.mode = Navigation.Mode.Explicit;
			
			if (i == 0) {
				button_nav.selectOnUp = detailContent.GetChild(detailContent.childCount - 1).GetComponent<Button>();
			} else {
				button_nav.selectOnUp = detailContent.GetChild(i - 1).GetComponent<Button>();
			}
			if (i == detailContent.childCount - 1) {
				button_nav.selectOnDown = detailContent.GetChild(0).GetComponent<Button>();
			} else {
				button_nav.selectOnDown = detailContent.GetChild(i + 1).GetComponent<Button>();
			}
			button.navigation = button_nav;
		}

		var eventSystem = FindObjectOfType<EventSystem>();

		detailContent.parent.parent.parent.GetComponent<Button>().onClick.AddListener(()=>eventSystem.SetSelectedGameObject(detailContent.GetChild(0).gameObject));

		//space = transform.GetChild(1).GetComponent<SphereCollider>();
	}

	public void setDestination(GameObject destination) {
		foreach (NavigationDetail d in details) {
			d.transform.GetChild(0).GetComponent<MeshRenderer>().material = nonSelectMaterial;
		}
		NavigationAgent.GetComponent<NavigationAgent>().toTarget = destination;
		destination.transform.GetChild(0).GetComponent<MeshRenderer>().material = selectMaterial;
		destination.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
	}



}
