using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーとカメラのルートとなるオブジェクトに付与するコンポーネント。
/// ポーズ状態の管理と、各オブジェクトに対する参照を主な役割としている。
/// </summary>
public class PlayerManager : ManagerMonoBehaviour<PlayerManager> {

	[SerializeField]
	//Playerの動作のポーズフラグ
	public bool is_pause;
	//Playerオブジェクト
	public GameObject player_model;
	//Cameraオブジェクト
	public GameObject player_camera;

	public void setPause(bool is_pause) {
		this.is_pause = is_pause;
	}

}
