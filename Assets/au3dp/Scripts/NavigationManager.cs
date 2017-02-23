using System;
using UnityEngine;
using System.Collections;

public class NavigationManager : MonoBehaviour {

	[SerializeField]
	public NavigationPlace[] places;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[Serializable]
	public class NavigationPlace {
		public GameObject navigationObject;
		public string navigationName;

	}

}
