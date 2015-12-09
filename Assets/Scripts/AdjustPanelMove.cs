using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdjustPanelMove : MonoBehaviour {

	
	[SerializeField]
	//座標情報取得
	private RectTransform rectTransform = null;
	//閉じてる状態（逆かも）
	private bool isClose = false;
	//開閉のスピード
	public float closeSpeed = 40.0f;
	//開閉アクションフラグ
	private bool isAction = false;

	/* Start
	 * 　（１）初期化
	 * 　（２）選択解除
	 */
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
		DetachChild ();
	}
	

	/* FixedUpdate
	 * 　（１）メニュー開閉ボタン押下で開閉アクション開始
	 * 　（２）開閉後、選択状態を変更
	 * 　（３）非アクティブ時は選択解除
	 */
	void FixedUpdate () {

		//メニュー開閉ボタン押下時
		if (Input.GetButtonDown ("Cancel") && (PlayerControllerScript.activeFlag || isClose==true)) {
			//Player動作とメニュー開閉を連動
			PlayerControllerScript.activeFlag = isClose;
			//開閉アクション開始
			isAction = true;
		}

		//開閉アクション
		if (isAction) {
			//開いてる時時
			if (isClose) {

				//閉じる処理
				if (rectTransform.anchoredPosition3D.x < 150) {
					rectTransform.anchoredPosition3D += new Vector3 (closeSpeed, 0, 0);
				} else {
					rectTransform.anchoredPosition3D = new Vector3 (150, 0, 0);
					isClose = false;
					isAction = false;
					DetachChild ();
				}
			//閉じてる時
			} else {
				//開く処理
				if (rectTransform.anchoredPosition3D.x > -150) {
					rectTransform.anchoredPosition3D -= new Vector3 (closeSpeed, 0, 0);
				} else {
					rectTransform.anchoredPosition3D = new Vector3 (-150, 0, 0);
					isClose = true;
					isAction = false;
					AttachChild ();
				}
			}
		//Playerが動いてる時は選択解除
		} else if(PlayerControllerScript.activeFlag) {
			if(!isClose)DetachChild ();
		}
	}

	/* 初期選択
	 * 　（１）子選択項目を走査し、タグがFirstSelectのものを探す
	 * 　（２）見つけたObjectを選択する。
	 */
	void AttachChild(){
		foreach(Transform child in transform){
			if(child.gameObject.tag=="FirstSelect"){
				(child.gameObject.GetComponent<Slider>()).Select ();
				break;
			}
		}
	}

	/* 選択解除
	 * 　（１）選択状態を全解除
	 */
	void DetachChild(){
		EventSystem.current.SetSelectedGameObject (null);
	}


}
