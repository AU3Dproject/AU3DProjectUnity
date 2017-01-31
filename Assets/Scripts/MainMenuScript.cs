using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    public Menu mainMenu;
	public EventSystem eventSystem;
	public GameObject first;
	public AudioClip seClip;
    private AudioSource se;

	// Use this for initialization
	void Start () {
        foreach (Transform child in mainMenu.transform.parent ) {
            if (child.gameObject.activeInHierarchy == true) {
                child.gameObject.SetActive(false);
            }
        }
		mainMenu.gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Menu")) {
            
			//閉じるとき
            if (mainMenu.gameObject.activeInHierarchy) {
				EventSystem.current.SetSelectedGameObject(null);
				foreach (Menu menu in mainMenu.transform.parent.GetComponentsInChildren<Menu>()) {
					menu.Close();
				}
                mainMenu.gameObject.SetActive(false);
				PlayerManager.Instance.is_pause = true;
				EventSystem.current.SetSelectedGameObject(null);
			//開くとき
			} else if(PlayerManager.Instance.is_pause) {
				mainMenu.gameObject.SetActive(true);
                if(first.activeInHierarchy) EventSystem.current.SetSelectedGameObject(first);
				PlayerManager.Instance.is_pause = false;
            }

        }

    }

}
