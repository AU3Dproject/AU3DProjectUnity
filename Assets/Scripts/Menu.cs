using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {

    [SerializeField]

	//戻る際のメニュー
    public Menu beforeMenu;
	//このメニューの最初に選択されるもの
    public GameObject firstSelect;
	//戻る際のショートカットキー
    public string backShortcutKey = "Cancel";

	public bool backBlock = false;

	//このメニューがアクティブかどうか（Activeなメニューは一つだけ）
    private bool isActive = false;


	/* このメニューを開く（beforeMenu無し）
	 * 	（１）メニューがアクティブ状態でなければ、
	 * 	（２）メニューのgameObjectをActiveにする。
	 * 	（３）beforeメニューの設定（Null）
	 * 	（４）初期選択の設定
	 * 	（５）アクティブにする。
	 */
    public void OpenMenu() {
        if (!isActive) {
            this.gameObject.SetActive(true);
            this.beforeMenu = null;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelect);
            isActive = true;
        }
    }

	/* このメニューを開く（beforeMenu有り）
	 * 	（１）メニューがアクティブ状態でなければ、
	 * 	（２）メニューのgameObjectをActiveにする。
	 * 	（３）beforeメニューの設定（引数）
	 * 	（４）初期選択の設定
	 * 	（５）アクティブにする。
	 */
    public void OpenMenu(Menu beforeMenu) {
        if (!isActive) {
            this.gameObject.SetActive(true);
            this.beforeMenu = beforeMenu;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelect);
            isActive = true;
        }
    }

	/* このメニューを閉じる
	 * 	（１）メニューがアクティブ状態であれば
	 * 	（２）beforeメニューのgameObjectをActiveにする。
	 * 	（３）beforeメニューの初期選択
	 * 	（４）このメニューのgameObjectを非Activeにする。
	 * 	（５）アクティブじゃなくする。
	 */
    public void Close() {
        if (isActive) {
            beforeMenu.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(beforeMenu.firstSelect);
            this.gameObject.SetActive(false);
            isActive = false;
        }
    }

	/* Update
	 * 	ショートカットキーを使って戻れるようにする。
	 */
    public void Update() {
        if (Input.GetButtonDown(backShortcutKey)) {
            Close();
        }
    }

	public void ApplicationQuit() {
		Application.Quit();
	}

	public void setBackBlock(bool block) {
		backBlock = block;
	}


}