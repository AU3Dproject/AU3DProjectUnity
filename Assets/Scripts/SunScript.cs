using UnityEngine;
using System.Collections;

public class SunScript : MonoBehaviour {

	[SerializeField]
	//角度
	public float angle = 0.0f;
	//移動スピード
	private float speed = 2.0f;
	//Lightコンポーネント
	private Light lightComponent ;
	//最大の光量
	private float maxIntensity = 2.0f;
	//動作させるかどうか
	public bool isMove = true;

	/* Start
	 * 　（１）初期化
	 */
	void Start () {
		lightComponent = this.GetComponent<Light>();
	}
	
	/* FixedUpdate
	 * 　（１）角度をスピードに応じて変化させていく
	 */
	void FixedUpdate () {

		//動かす時
		if (isMove) {
			//角度を徐々に変化
			angle += speed * Time.deltaTime;
			//角度をループ
			if (angle > 360.0f)
				angle -= 360.0f;
			//光量を角度によって徐々に変化
			lightComponent.intensity = maxIntensity * (Mathf.Sin (angle * Mathf.Deg2Rad));

			//角度の反映
			this.transform.localRotation = Quaternion.Euler (new Vector3 (angle, 0, 0));

		}
	}

	//スピード調整
	public void setSunSpeed(float value){
		this.speed = value;
	}
	//動かすかどうか
	public void setMovable(bool value){
		this.isMove = value;
	}

}
