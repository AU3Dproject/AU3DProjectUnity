using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// メニューのマップに使うカメラのスクリプト
/// </summary>
public class MenuMapCamera : MonoBehaviour {

	//マップマネージャ
	Transform mapManager;
	//カメラによって選択している、マップにある場所オブジェクトに付随するコライダー
	Collider selectedCollider = null;

	// Use this for initialization
	void Start() {
		// mapManager = MapManager.Instance.transform;
	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// カメラがどこかの場所を選択した瞬間の処理
	/// </summary>
	/// <param name="other">選択した場所に付随するコライダー</param>
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == 11) {
			selectedCollider = other;
			mapPlaneSelect(selectedCollider, Color.blue);
			buttonSelect(selectedCollider, Color.yellow);
		}
	}

	/// <summary>
	/// カメラがどこかの場所の選択をしなくなった瞬間
	/// </summary>
	/// <param name="other">選択をしなくした場所に付随するコライダー</param>
	void OnTriggerExit(Collider other) {
		if (other.gameObject.layer == 11) {
			mapPlaneSelect(other, Color.red);
			buttonSelect(other,Color.white);
			selectedCollider = null;
		}
	}

	/// <summary>
	/// マップの特定の場所を示すPlaneオブジェクトの色替え
	/// </summary>
	/// <param name="other">場所を示すPlaneオブジェクトのコライダー</param>
	/// <param name="color">指定する色</param>
	void mapPlaneSelect(Collider other, Color color) {
		other.gameObject.GetComponent<Renderer>().material.color = color;
	}

	/// <summary>
	/// 場所を示すボタンの色替え
	/// </summary>
	/// <param name="other">ボタンのコライダー</param>
	/// <param name="color">指定する色</param>
	void buttonSelect(Collider other ,Color color) {
		int index = -1;
		//ボタンを総当りし引数のコライダーと同じだった場合、それの色替えをする。
		for (int i = 0; i < mapManager.childCount; i++) {
			if (mapManager.GetChild(i) == other.transform.parent) {
				index = i;
				break;
			}
		}
		//引数で指定したボタンが見つかった時、そのボタンの画像の色替えをする。
		if (index != -1) {
			GameObject nav_content = GameObject.Find("ScrollPanel").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(index).gameObject;
			nav_content.GetComponent<Image>().color = color;
		}
	}
}
