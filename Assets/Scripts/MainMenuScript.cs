using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    public Canvas menuCanvas;
	public EventSystem eventSystem;
	public GameObject first;

	// Use this for initialization
	void Start () {
		menuCanvas.gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Menu")) {
            if (menuCanvas.gameObject.activeInHierarchy) {
                menuCanvas.gameObject.SetActive(false);
            } else {
                menuCanvas.gameObject.SetActive(true);
				eventSystem.SetSelectedGameObject (first);
            }

        }
	}
}
