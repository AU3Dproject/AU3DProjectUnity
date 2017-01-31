using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	
	[SerializeField]
	//PlayerとCameraの距離
	public float distance = 1.2f ;
	//Cameraの水平方向の角度
	public float HorizontalAngle = Mathf.PI ;
	//Cameraの垂直方向の角度
	public float VerticalAngle = 0.0f ;
	//Cameraの位置的高さ
	public float tall = 1.0f;
	//Cameraの角度調整時の速度
	public float angle_speed = 2.0f;
	//Cameraの拡大率
	public float zoom = 0.0f;
	//三人称視点フラグ
	public bool isTP = true;
	//三人称視点の水平方向角度の制限(度数法)
	public Vector2 HorizontalAngleLimitTP = new Vector2(0.0f,360.0f);
	//三人称視点の並行方向角度の制限(度数法)
	public Vector2 VerticalAngleLimitTP = new Vector2(-25.0f,25.0f);
	//一人称視点の水平方向角度の制限(度数法)
	public Vector2 HorizontalAngleLimitFP = new Vector2(0.0f,360.0f);
	//一人称視点の並行方向角度の制限(度数法)
	public Vector2 VerticalAngleLimitFP = new Vector2(-25.0f,25.0f);

	//三人称視点時のPlayerモデル不可視化のためのPlayer表示コンポーネント取得
	private SkinnedMeshRenderer playerModel = null;
	//Playerオブジェクト
	private GameObject Player = null;
	//Cameraコンポーネント
	private Camera cameraComponent = null;
	//Cameraの壁めり込み判定のためのRaycast
	private RaycastHit hit;
	
	/* Start
	 * 　（１）Component・Objectの取得
	 */
	public void Start () {
		cameraComponent = GetComponent<Camera> ();
		Player = PlayerManager.Instance.player_model;
		playerModel = Player.GetComponentInChildren<SkinnedMeshRenderer>();
	}

	
	/* FixedUpdate
	 * 　（１）三人称視点モードにより分岐
	 * 　（２）カメラ動作処理
	 */
	public void Update () {
		if (PlayerManager.Instance.is_pause) {
			UpdateCamera();
		}
	}


	/* カメラ処理
	 * 　（１）拡大率・角度調整
	 * 　（２）拡大率・角度制限
	 * 　（３）新しい座標値計算と反映
	 * 　（４）新しい角度の反映
	 * 　（５）めり込み判定と調整
	 */
	private void UpdateCamera(){

		//拡大率調整
		zoom += Input.GetAxis ("Zoom") * 0.1f;
		//水平・垂直角度調整
		HorizontalAngle += Mathf.Deg2Rad * angle_speed * Input.GetAxisRaw ("CameraHorizontal");
		VerticalAngle += Mathf.Deg2Rad * angle_speed * Input.GetAxisRaw ("CameraVertical");

		//水平角度の数値上の制限
		if (HorizontalAngle <= (isTP ? HorizontalAngleLimitTP.x : HorizontalAngleLimitFP.x) * Mathf.Deg2Rad)
			HorizontalAngle += (isTP ? HorizontalAngleLimitTP.y : HorizontalAngleLimitFP.y) * Mathf.Deg2Rad;
		if (HorizontalAngle >= (isTP ? HorizontalAngleLimitTP.y : HorizontalAngleLimitFP.y) * Mathf.Deg2Rad)
			HorizontalAngle -= (isTP ? HorizontalAngleLimitTP.y : HorizontalAngleLimitFP.y) * Mathf.Deg2Rad;
		//垂直角度の制限
		if (VerticalAngle <= (isTP ? VerticalAngleLimitTP.x : VerticalAngleLimitFP.x) * Mathf.Deg2Rad)
			VerticalAngle =  (isTP ? VerticalAngleLimitTP.x : VerticalAngleLimitFP.x) * Mathf.Deg2Rad;
		if (VerticalAngle >= (isTP ? VerticalAngleLimitTP.y : VerticalAngleLimitFP.y) * Mathf.Deg2Rad)
			VerticalAngle =  (isTP ? VerticalAngleLimitTP.y : VerticalAngleLimitFP.y) * Mathf.Deg2Rad;
		//拡大率の制限
		if (zoom <= -distance / 2)zoom = -distance / 2;
		if (zoom >= +distance * 2)zoom = +distance * 2;

		//新しい各座標値の計算
		Vector3 newPos = new Vector3 ();
		if (isTP) {
			newPos.x = - (distance + zoom) * Mathf.Sin (HorizontalAngle) - Mathf.Cos (VerticalAngle) * Mathf.Sin (HorizontalAngle) + Player.transform.position.x;
			newPos.y = + (distance + zoom) * Mathf.Sin (VerticalAngle) + (Player.transform.position.y + tall);
			newPos.z = - (distance + zoom) * Mathf.Cos (HorizontalAngle) - Mathf.Cos (VerticalAngle) * Mathf.Cos (HorizontalAngle) + Player.transform.position.z;
		} else {
			newPos.x = - (distance) * Mathf.Sin (HorizontalAngle) + Player.transform.position.x;
			newPos.y = +  tall + Player.transform.position.y;
			newPos.z = - (distance) * Mathf.Cos (HorizontalAngle) + Player.transform.position.z;
		}
		//新座標値の反映
		transform.position = newPos;

		//CameraをPlayer方向へ向ける
		transform.LookAt (Player.transform.position + new Vector3(0.0f,tall,0.0f));
		//Cameraをx軸方向を調整
		if (!isTP) {
			float new_rotation_x = transform.localRotation.eulerAngles.x + VerticalAngle * Mathf.Rad2Deg;
			float new_rotation_y = transform.localRotation.eulerAngles.y;
			float new_rotation_z = transform.localRotation.eulerAngles.z;
			this.transform.localRotation = Quaternion.Euler (new Vector3 (-new_rotation_x, new_rotation_y, new_rotation_z));
		}

		//Cameraの壁めり込み判定と調整
		Vector3 ptv = Player.transform.position + new Vector3 (0, tall, 0);
		Vector3 normal = (transform.position - ptv).normalized;
		if (Physics.Raycast (ptv, normal, out hit,distance + zoom,1)) {
			transform.position = hit.point;
		}

	}



	//角度調整スピード変更
	public void setAngleSpeed(float value){
		angle_speed = value;
	}
	//Cameraの位置的高さの変更
	public void setTall(float value){
		tall = value;
		distance = value;
	}
	//三人称視点と一人称視点の切替
	public void setTPMode(bool value){
		isTP=!value;
		if (!isTP) {
			distance = 0.1f;
			tall = 1.2f;
			if(playerModel != null)playerModel.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
		} else {
			distance = 1.2f;
			tall = 1.0f;
			if(playerModel != null)playerModel.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}
	}

	
}
