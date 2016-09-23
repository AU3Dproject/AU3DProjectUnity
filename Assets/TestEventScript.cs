using UnityEngine;
using System.Collections;

public class TestEventScript : MonoBehaviour {

	public GameObject textWindow;
	public float time = 5.0f;
	GameObject canvas;

	// Use this for initialization
	void Start () {
		canvas = (GameObject)Instantiate(textWindow, new Vector3(0, 0, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		if(time>=0) {
			time -= Time.deltaTime;
		}else {
			canvas.transform.GetChild(0).GetComponent<Writer>().isTextVisible = true;
			this.gameObject.SetActive (false);
		}
	}
}
