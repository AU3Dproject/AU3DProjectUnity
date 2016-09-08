using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    public Canvas menuCanvas;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Menu")) {
            if (menuCanvas.gameObject.activeInHierarchy) {
                menuCanvas.gameObject.SetActive(false);
            } else {
                menuCanvas.gameObject.SetActive(true);
            }

        }
	}
}
