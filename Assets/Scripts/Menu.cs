using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {

    [SerializeField]
    public Menu beforeMenu;
    public EventSystem eventSystem;
    public GameObject firstSelect;

    public void OpenMenu(Menu targetMenu) {
        beforeMenu = targetMenu;

    }

    public void Close() {
        beforeMenu.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(beforeMenu.firstSelect);
        
    }

    public virtual void onClose() { }

}