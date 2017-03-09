using UnityEngine;
using System.Collections;

public class TPCamera : MonoBehaviour {

	#region public

	[SerializeField]
	//PlayerとCameraの距離
	public float distance = 2.0f ;
	//Cameraの水平方向の角度
	public float horizontal_angle = 180.0f ;
	//Cameraの垂直方向の角度
	public float vertical_angle = 0.0f ;
	//Cameraの位置的高さ
	public float tall = 1.0f;
	//Cameraの角度調整時の速度
	public float angle_speed = 2.0f;
	//Cameraの拡大率
	public float zoom = 0.0f;
	[Tooltip("カメラの拡大スピード")]
	public float zoom_speed = 0.1f;
	//水平方向角度の制限(度数法)
	public Vector2 horizontal_angle_limit = new Vector2(0.0f,360.0f);
	public Vector2 vertical_angle_limit = new Vector2(-25.0f,25.0f);
	[Tooltip("カメラの拡大率の制限")]
	public Vector2 zoom_limit = new Vector2(0.0f,4.0f);

	#endregion

	#region private

	//三人称視点時のPlayerモデル不可視化のためのPlayer表示コンポーネント取得
	private SkinnedMeshRenderer playerModel = null;
	//Playerオブジェクト
	private GameObject Player = null;
	//Cameraコンポーネント
	private Camera cameraComponent = null;
	//Cameraの壁めり込み判定のためのRaycast
	private RaycastHit hit;

	#endregion

	#region method

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
		if (!PlayerManager.Instance.is_pause) {
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
		zoom += Input.GetAxis ("Zoom") * zoom_speed;

		//水平・垂直角度調整
		horizontal_angle += angle_speed * Input.GetAxisRaw ("CameraHorizontal");
		vertical_angle += angle_speed * Input.GetAxisRaw ("CameraVertical");

		//垂直角度の制限
		horizontal_angle = calc_limit_loop(horizontal_angle, horizontal_angle_limit);
		vertical_angle = calc_limit(vertical_angle, vertical_angle_limit);

		//拡大率の制限
		zoom = calc_limit(zoom, zoom_limit);
		
		//新座標値の反映
		transform.position = calc_position();

		//CameraをPlayer方向へ向ける
		transform.LookAt (Player.transform.position + new Vector3(0.0f,tall,0.0f));

		//Cameraの壁めり込み判定と調整
		Vector3 ptv = Player.transform.position + new Vector3(0, tall, 0);
		Vector3 normal = (transform.position - ptv).normalized;
		LayerMask mask = (1 << 9) + (1 << 10);
		if (Physics.Raycast(ptv, normal, out hit, distance + zoom, mask)) {
			transform.position = hit.point;
		}

	}

	private Vector3 calc_position() {
		float rad_H = horizontal_angle * Mathf.Deg2Rad;
		float rad_V = vertical_angle * Mathf.Deg2Rad;
		Vector3 newPos = new Vector3();
		newPos.x = -(distance + zoom) * Mathf.Sin(rad_H) - Mathf.Cos(rad_V) * Mathf.Sin(rad_H) + Player.transform.position.x;
		newPos.y = +(distance + zoom) * Mathf.Sin(rad_V) + (Player.transform.position.y + tall);
		newPos.z = -(distance + zoom) * Mathf.Cos(rad_H) - Mathf.Cos(rad_V) * Mathf.Cos(rad_H) + Player.transform.position.z;

		return newPos;
	}

	private float calc_limit(float value, Vector2 limit) {
		float min = Mathf.Min(limit.x,limit.y) , max = Mathf.Max(limit.x,limit.y);
		if (value < min) {
			value = min;
		}else if (value > max) {
			value = max;
		}
		return value;
	}
	private float calc_limit_loop(float value, Vector2 limit) {
		float min = Mathf.Min(limit.x, limit.y), max = Mathf.Max(limit.x, limit.y);
		if (value < min) {
			value += max;
		} else if (value > max) {
			value -= max;
		}
		return value;
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

	#endregion

}
