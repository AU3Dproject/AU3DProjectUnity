using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    public Canvas menuCanvas;
	public EventSystem eventSystem;
	public GameObject first;
    private AudioSource se;

	// Use this for initialization
	void Start () {
		menuCanvas.gameObject.SetActive(false);
        se = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Menu")) {
            
			//閉じるとき
            if (menuCanvas.gameObject.activeInHierarchy) {
				se.PlayOneShot(se.clip);
				EventSystem.current.SetSelectedGameObject(null);
                menuCanvas.gameObject.SetActive(false);
                PlayerControllerScript.activeFlag = true;
			//開くとき
			} else if(PlayerControllerScript.activeFlag) {
				se.PlayOneShot(se.clip);
				menuCanvas.gameObject.SetActive(true);
                if(first.activeInHierarchy) EventSystem.current.SetSelectedGameObject(first);
                PlayerControllerScript.activeFlag = false;
            }

        }

    }

}
