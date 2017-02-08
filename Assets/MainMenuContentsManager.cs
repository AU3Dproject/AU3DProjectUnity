using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuContentsManager : MonoBehaviour {

	[SerializeField]
	public float moving_distance = 3100.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FocusContents(int index) {

		transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(-moving_distance * index,0), 0.4f).SetEase(Ease.InOutQuart);
		
	}

}
