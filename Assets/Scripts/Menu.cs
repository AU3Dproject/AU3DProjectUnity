using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {

    [SerializeField]
    public Menu beforeMenu;
    public EventSystem eventSystem;
    public GameObject firstSelect;
    public string backShortcutKey = "Cancel";

    private bool isActive = false;

    public void OpenMenu() {
        if (!isActive) {
            this.gameObject.SetActive(true);
            this.beforeMenu = null;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelect);
            isActive = true;
        }
    }

    public void OpenMenu(Menu beforeMenu) {
        if (!isActive) {
            this.gameObject.SetActive(true);
            this.beforeMenu = beforeMenu;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelect);
            isActive = true;
        }
    }

    public void Close() {
        if (isActive) {
            beforeMenu.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(beforeMenu.firstSelect);
            this.gameObject.SetActive(false);
            isActive = false;
        }
    }

    public void Update() {
        if (Input.GetButtonDown(backShortcutKey)) {
            Close();
        }
    }


}