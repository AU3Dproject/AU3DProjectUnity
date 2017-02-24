using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {
	
	[SerializeField]
	[Tooltip("歩行速度")]
	public float walk_speed = 4.0f;
	[Tooltip("走行速度")]
	public float dash_speed = 20.0f;
	[Tooltip("歩行・走行速度調整(歩行・走行速度に加算)")]
	public float adjust_speed = 0.0f;
	[Tooltip("重力")]
	public const float gravity = 30.0f;
	[Tooltip("ジャンプ力")]
	public float jump_power = 10.0f;
	[Tooltip("初期位置")]
	public Vector3 initialize_position = new Vector3 (-413, 0, 330);
	[Tooltip("常時ダッシュフラグ")]
	public bool always_dash_flag = false;
	[Tooltip("ナビゲーションエージェント")]
	public NavigationAgent navigation_agent;

	//フラーム移動ベクトル
	private Vector3 move_vector ;
	//CharacterContorollerオブジェクト
	private CharacterController character_controller = null;
	//Animatorコンポーネント
	private Animator animator = null;
    //ジャンプのフラグ
    private bool is_jump = false;
	

	/* Start
	 * 　（１）変数・コンポーネント初期化
	 */
	public void Start () {
		character_controller = GetComponent<CharacterController> ();
		animator = GetComponent<Animator>();
		move_vector = new Vector3(0.0f,0.0f,0.0f);
	}
	

	/* Update
	 * 　（１）移動処理総合
	 */
	public void Update () {
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
		if (!PlayerManager.Instance.is_pause) {
			moveInit ();
			moveJump ();
			moveWalking ();
			character_controller.Move (move_vector * Time.deltaTime);
		} else {
			settingAnimator(true,false,false,false);
		}
	}


	/* ジャンプ処理
	 * 　（１）接地していたらジャンプさせる
	 * 　（２）Jumpボタン押下時、Y方向に移動してない場合、ジャンプさせる。
	 * 　（３）ジャンプする際、着地の際にAnimatorを変化させる。
	 */
	 //todo: コード汚い
	private void moveJump(){
		
		if (character_controller.isGrounded) {
			if(animator.GetBool ("isJump")==true && move_vector.y == 0.0f){
				settingAnimator(null,null,null,false);
			}
			if(Input.GetButton("Jump") && !is_jump){
				is_jump = true;
				move_vector.y+=(jump_power);
				settingAnimator(null,null,null,true);
            }
			if(!Input.GetButton("Jump") && !animator.GetBool("isJump")) {
				is_jump = false;
				navigation_agent.warpAgent(transform.position);
			}
		}
	}


	/* 初期位置移動
	 * 　（１）初期位置ボタン押下時、またはY座標が-10以下（落下時）に初期位置に戻す
	 * 　（２）Animatorも初期化
	 */
	private void moveInit(){
		if (Input.GetButtonDown("Init") || transform.position.y <= -10.0f) {
			settingAnimator(true,false,false,false);
			transform.position = initialize_position;
		}
	}


	/* 重力処理
	 * 　（１）接地していない時は重力を下方向にかける。
	 * 　（２）設置しているときは重力をかけない。
	 */
	private void moveGravity(){
		if (character_controller.isGrounded) {
			move_vector.y = 0.0f;
		} else {
			move_vector.y -= gravity * Time.deltaTime;
		}
	}


	/* 移動処理
	 * 　（１）x,z方向の移動ベクトルの計算と処理
	 * 　（２）Animatorの切替を行う
	 * 　（３）移動中にキャラクターの方向を移動方向に同期させる
	 */
	private void moveWalking(){

		//歩行・走行スピード一時変数
		float spd = (Input.GetButton ("Dash") ^ always_dash_flag) ? dash_speed : walk_speed;
		//カメラの直進方向の一時変数
		float angle = PlayerManager.Instance.player_camera.transform.localEulerAngles.y;
		//x方向とz方向の移動の0~1の数値
		float mv_z = (Input.GetAxis ("MoveVertical") * Mathf.Cos (Mathf.Deg2Rad * angle) + Input.GetAxis ("MoveHorizontal") * Mathf.Sin (Mathf.Deg2Rad * (angle + 180.0f)));
		float mv_x = (Input.GetAxis ("MoveVertical") * Mathf.Sin (Mathf.Deg2Rad * angle) - Input.GetAxis ("MoveHorizontal") * Mathf.Cos (Mathf.Deg2Rad * (angle + 180.0f)));
		//x方向とz方向の移動のスピードと調整スピードの反映
		mv_z = mv_z * (spd + adjust_speed);
		mv_x = mv_x * (spd + adjust_speed);
		//移動ベクトルの決定
		move_vector = new Vector3 (mv_x,move_vector.y,mv_z);

		//X方向かZ方向に動いている時
		if (mv_z != 0.0f || mv_x != 0.0f) {
			//歩行・走行アニメーション切替
			if (Input.GetButton ("Dash") ^ always_dash_flag && animator.GetBool("isJump") == false) {
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


    //privateメソッド

    /* Animator設定
	 * 　（１）PlayerAnimatorで用いる4状態のboolを変更する
	 * 　（２）変更せず現状維持させる場合、引数にnullを入れる
	 */
    private void settingAnimator(bool? stay,bool? walk,bool? run,bool? jump){
		if (animator != null) {
			if(stay!=null)animator.SetBool ("isStaying", (bool)stay);
			if(walk!=null)animator.SetBool ("isWalking", (bool)walk);
			if(run !=null)animator.SetBool ("isRunning", (bool)run );
			if(jump!=null)animator.SetBool ("isJump"   , (bool)jump);
		}
	}




	//調整

	//速度調整
	public void setAdjustSpeed(float value){
		adjust_speed = value;
	}

	//ジャンプ力調整
	public void setJumpPower(float value){
		jump_power = value;
	}

	//スケール調整
	public void setScale(float value){
		transform.localScale = new Vector3 (value,value,value);
	}

}
