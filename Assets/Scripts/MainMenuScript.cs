using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    public Canvas menuCanvas;
	public EventSystem eventSystem;
	public GameObject first;
	public AudioClip seClip;
    private AudioSource se;

	// Use this for initialization
	void Start () {
		menuCanvas.gameObject.SetActive(false);
		se = GameObject.Find("Manager").GetComponent<Manager>().SEManager.GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Menu")) {
            
			//閉じるとき
            if (menuCanvas.gameObject.activeInHierarchy) {
				se.PlayOneShot(seClip);
				EventSystem.current.SetSelectedGameObject(null);
                menuCanvas.gameObject.SetActive(false);
                PlayerControllerScript.activeFlag = true;
			//開くとき
			} else if(PlayerControllerScript.activeFlag) {
				se.PlayOneShot(seClip);
				menuCanvas.gameObject.SetActive(true);
                if(first.activeInHierarchy) EventSystem.current.SetSelectedGameObject(first);
                PlayerControllerScript.activeFlag = false;
            }

        }

    }

}
