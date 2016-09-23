using UnityEngine;
using System.Collections;

public class TestEventScript : MonoBehaviour {

	public GameObject textWindow;
	public float time = 5.0f;
	GameObject canvas;
	int mode = 0;

	// Use this for initialization
	void Start () {
		canvas = (GameObject)Instantiate(textWindow, new Vector3(0, 0, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		if (mode == 0) {
			if (time >= 0) {
				time -= Time.deltaTime;
			} else {
				canvas.transform.GetChild (0).GetComponent<Writer> ().isTextActive = true;
				mode = 1;
			}
		} else if (mode == 1) {
			if (Input.GetKeyDown (KeyCode.O)) {
				canvas.transform.GetChild (0).GetComponent<Writer> ().allVisible ();
			}
			if (!canvas.transform.GetChild (0).GetComponent<Writer> ().isTextActive) {
				mode = 2;
			}
		} else if (mode == 2) {
			if (Input.GetKeyDown (KeyCode.O)) {
				canvas.transform.GetChild (0).GetComponent<Writer> ().removeText ();
				mode = 3;
			}
		} else if (mode == 3) {
			if (Input.GetKeyDown (KeyCode.O)) {
				canvas.transform.GetChild (0).GetComponent<Writer> ().removeWindow ();
				mode = 4;
			}
		} else if (mode == 4) {
			if (Input.GetKeyDown (KeyCode.O)) {
				canvas = (GameObject)Instantiate(textWindow, new Vector3(0, 0, 0), Quaternion.identity);
				time = 0;
				mode = 0;
			}

		}
	}
}
