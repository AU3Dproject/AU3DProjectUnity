using UnityEngine;
using System.Collections;
using Fungus;

public class CharacterMove : MonoBehaviour {
	
	[SerializeField]
	//歩行速度
	public float Speed = 4.0f;
	//走行速度
	public float DashSpeed = 20.0f;
	//歩行・走行速度調整
	public float AdjustSpeed = 0.0f;
	//重力
	public float Gravity = 30.0f;
	//ジャンプ力
	public float JumpPower = 10.0f;
	//接地判定調整
	public float tolerance = 0.5f;
	//初期位置
	public Vector3 initPosition = new Vector3 (-413, 0, 330);
	//常時ダッシュフラグ
	public bool AlwaysDashMode = false;
	//カメラオブジェクト
	public GameObject Camera=null;
	//Animator仕様フラグ
	public bool useAnimator = false;
    //足音
    public AudioClip WalkSE;
    //ジャンプ音
    public AudioClip JumpSE;

    //フラーム移動ベクトル
    private Vector3 move_vector ;
	//CharacterContorollerオブジェクト
	private CharacterController characterController = null;
	//Animatorコンポーネント
	private Animator animator = null;
    //ジャンプのフラグ
    private bool isJump = false;
	
	/* Start
	 * 　（１）変数・コンポーネント初期化
	 */
	public void Start () {
		characterController = this.GetComponent<CharacterController> ();
		if(useAnimator)this.animator = this.GetComponent<Animator>();
		move_vector = new Vector3(0.0f,0.0f,0.0f);
	}
	

	/* FixedUpdate
	 * 　（１）移動処理総合
	 */
	public void FixedUpdate () {
		move ();
	}







	/* 移動処理総合
	 * 　（１）重力・初期位置移動・ジャンプ・歩行の各処理を行う
	 * 　（２）各処理後、Move関数でmove_vector変数を使い、移動を実現する
	 * 　（３）PlayerCotrollerScriptのactivgeFlagによって移動制限がされる
	 * 　（４）移動制限時は直立させるため、Animatorを初期化
	 */
	private void move(){
		moveGravity ();
		if (PlayerControllerScript.activeFlag) {
			moveInit ();
			moveJump ();
			moveWalking ();
			characterController.Move (move_vector * Time.deltaTime);
		} else {
			settingAnimator(true,false,false,false);
		}
	}


	/* ジャンプ処理
	 * 　（１）接地していたらジャンプさせる
	 * 　（２）Jumpボタン押下時、Y方向に移動してない場合、ジャンプさせる。
	 * 　（３）ジャンプする際、着地の際にAnimatorを変化させる。
	 */
	private void moveJump(){
		if (CheckGrounded()) {
			if(this.animator.GetBool ("isJump")==true && move_vector.y == 0.0f){
				settingAnimator(null,null,null,false);
			}
			if(Input.GetButton("Jump")){
				move_vector.y+=(JumpPower);
				settingAnimator(null,null,null,true);
                AudioSource.PlayClipAtPoint(JumpSE, animator.gameObject.transform.position);
            }
		}
	}


	/* 初期位置移動
	 * 　（１）初期位置ボタン押下時、またはY座標が-10以下（落下時）に初期位置に戻す
	 * 　（２）Animatorも初期化
	 */
	private void moveInit(){
		if (Input.GetButtonDown("Init") || this.transform.position.y <= -10.0f) {
			settingAnimator(true,false,false,false);
			this.transform.position = initPosition;
		}
	}


	/* 重力処理
	 * 　（１）接地していない時は重力を下方向にかける。
	 * 　（２）設置しているときは重力をかけない。
	 */
	private void moveGravity(){
		if (!CheckGrounded()) {
			move_vector.y -= Gravity * Time.deltaTime;
		} else {
			move_vector.y = 0.0f;
		}
	}


	/* 移動処理
	 * 　（１）x,z方向の移動ベクトルの計算と処理
	 * 　（２）Animatorの切替を行う
	 * 　（３）移動中にキャラクターの方向を移動方向に同期させる
	 */
	private void moveWalking(){

		//歩行・走行スピード一時変数
		float spd = Input.GetButton ("Dash") ^ AlwaysDashMode ? DashSpeed : Speed;
		//カメラの直進方向の一時変数
		float angle = Camera.transform.localEulerAngles.y;
		//x方向とz方向の移動の0~1の数値
		float mv_z = (Input.GetAxis ("MoveVertical") * Mathf.Cos (Mathf.Deg2Rad * angle) + Input.GetAxis ("MoveHorizontal") * Mathf.Sin (Mathf.Deg2Rad * (angle + 180.0f)));
		float mv_x = (Input.GetAxis ("MoveVertical") * Mathf.Sin (Mathf.Deg2Rad * angle) - Input.GetAxis ("MoveHorizontal") * Mathf.Cos (Mathf.Deg2Rad * (angle + 180.0f)));
		//x方向とz方向の移動のスピードと調整スピードの反映
		mv_z = mv_z * (spd + AdjustSpeed);
		mv_x = mv_x * (spd + AdjustSpeed);
		//移動ベクトルの決定
		move_vector = new Vector3 (mv_x,move_vector.y,mv_z);

		//X方向かZ方向に動いている時
		if (mv_z != 0.0f || mv_x != 0.0f) {
			//歩行・走行アニメーション切替
			if (Input.GetButton ("Dash") ^ AlwaysDashMode && this.animator.GetBool("isJump") == false) {
				//Run状態へ
				settingAnimator(false,false,true,null);
			}else{
				//Walk状態へ
				settingAnimator(false,true,false,null);
			}

			//進行方向にPlayerを向かせる処理
			Vector3 angle_vector = move_vector;
			angle_vector.y = 0;
			Quaternion characterTargetRotation = Quaternion.LookRotation (angle_vector);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, characterTargetRotation, 360 * 2 * Time.deltaTime);
		
		//動いていない時
		} else {
			//Stay状態へ
			settingAnimator(true,false,false,null);
		}
	}


    public void WalkSound(){
        AudioSource.PlayClipAtPoint(WalkSE,animator.gameObject.transform.position);
        
    }


    //privateメソッド

    /* Animator設定
	 * 　（１）PlayerAnimatorで用いる4状態のboolを変更する
	 * 　（２）変更せず現状維持させる場合、引数にnullを入れる
	 */
    private void settingAnimator(bool? stay,bool? walk,bool? run,bool? jump){
		if (useAnimator) {
			if(stay!=null)this.animator.SetBool ("isStaying", (bool)stay);
			if(walk!=null)this.animator.SetBool ("isWalking", (bool)walk);
			if(run !=null)this.animator.SetBool ("isRunning", (bool)run );
			if(jump!=null)this.animator.SetBool ("isJump"   , (bool)jump);
		}
	}




	//調整

	//速度調整
	public void setAdjustSpeed(float value){
		this.AdjustSpeed = value;
	}

	//ジャンプ力調整
	public void setJumpPower(float value){
		this.JumpPower = value;
	}

	//スケール調整
	public void setScale(float value){
		this.transform.localScale = new Vector3 (value,value,value);
	}




	//改善

	//接地改善
	//http://www.section31.x0.com/gamedevelopment/unity/unity-charactercontrolle-isgrounded%E3%81%AE%E7%B2%BE%E5%BA%A6%E5%90%91%E4%B8%8A%E3%81%AB%E3%81%A4%E3%81%84%E3%81%A6/
	bool CheckGrounded(){
		//CharacterControlle.IsGroundedでまずは判定
	/*	if (this.characterController.isGrounded){
			return true; 
		}
		
		//MASK　自機以外判定
		int layerMask = (1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Water"));
		layerMask = ~layerMask;
		
		//Raycastして距離を測定(5メートル以内で判定
		RaycastHit hit;
		if (Physics.Raycast(this.characterController.transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 5f , layerMask)){
			if (hit.distance > tolerance){	//tolerance以内の距離なら接地
				return false;
			}else{
				return true;
			}
		}else{
			return true;
		}*/
		return characterController.isGrounded;
	}

}
