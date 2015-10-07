using UnityEngine;
using System.Collections;
using Fungus;

public class CharacterMove : MonoBehaviour {
	
	[SerializeField]
	public float Speed = 4.0f;
	public float DashSpeed = 12.0f;
	public float AdjustSpeed = 0.0f;
	public float Gravity = 30.0f;
	public float JumpPower = 10.0f;
	public float tolerance = 0.5f;
	public Vector3 initPosition = new Vector3 (-413, 0, 330);
	public bool DashButtonMode = false;
	public GameObject Camera=null;
	public bool isTP = false;
	public bool useAnimator = false;
	public Flowchart flowchart ;
	
	private Vector3 move_vector ;
	private CharacterController characterController = null;
	private Animator animator = null;
	//private MMD4MecanimModel model = null;
	private AnimatorStateInfo animatorState;
	
	// Use this for initialization
	public void Start () {
		characterController = this.GetComponent<CharacterController> ();
		//model = this.GetComponent<MMD4MecanimModel>();
		if(useAnimator)this.animator = this.GetComponent<Animator>();
		if (useAnimator)this.animatorState = this.animator.GetCurrentAnimatorStateInfo (0);
		move_vector = new Vector3(0.0f,0.0f,0.0f);
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		move ();
	}
	
	private void moveJump(){
		if (CheckGrounded()) {
			if(useAnimator && this.animator.GetBool ("isJump")==true && move_vector.y == 0.0f){
				this.animator.SetBool ("isJump",false);
			}
			if(Input.GetButton("Jump") && move_vector.y == 0.0f){
				move_vector.y+=(JumpPower);
				if(useAnimator)this.animator.SetBool ("isJump", true);
			}
		}
	}
	
	private void moveInit(){
		if (Input.GetButtonDown("Init") || this.transform.position.y <= -10.0f) {
			if (useAnimator){
				Debug.Log ("init");
				this.animator.SetBool ("isJump",false);
				this.animator.SetBool ("isWalking", false);
				this.animator.SetBool ("isRunning", false);
				this.animator.SetBool ("isStaying", true);
			}
			this.transform.localPosition = initPosition;
		}
	}
	
	private void moveGravity(){
		if (!CheckGrounded()) {
			move_vector.y -= Gravity * Time.deltaTime;
		} else {
			move_vector.y = 0.0f;
		}
	}
	private void moveWalking(){
		float spd = 0.0f;
		if (DashButtonMode) {
			if (Input.GetButton ("Dash")) {
				spd = DashSpeed + AdjustSpeed;
			} else {
				spd = Speed + AdjustSpeed;
			}
		} else {
			if (!Input.GetButton ("Dash")) {
				spd = DashSpeed + AdjustSpeed;
			} else {
				spd = Speed + AdjustSpeed;
			}
		}
		
		float angle = Camera.transform.localEulerAngles.y;
		
		float mv_z = (  Input.GetAxis ("Vertical") * Mathf.Cos (Mathf.Deg2Rad * angle) + Input.GetAxis ("Horizontal") * Mathf.Sin (Mathf.Deg2Rad * (angle+180.0f)) ) * spd;
		float mv_x = (  Input.GetAxis ("Vertical") * Mathf.Sin (Mathf.Deg2Rad * angle) - Input.GetAxis ("Horizontal") * Mathf.Cos (Mathf.Deg2Rad * (angle+180.0f)) ) * spd;
		move_vector = new Vector3 (mv_x,move_vector.y,mv_z);

		if (mv_z != 0.0f || mv_x != 0.0f) {
			if (spd <= DashSpeed + AdjustSpeed) {
				if (useAnimator && this.animator.GetBool("isJump") == false){
					this.animator.SetBool ("isWalking", true);
					this.animator.SetBool ("isRunning", false);
				}
			}
			if (spd >= DashSpeed + AdjustSpeed) {
				if (useAnimator && this.animator.GetBool("isJump") == false){
					this.animator.SetBool ("isRunning", true);
					this.animator.SetBool ("isWalking", false);
				}
			}
			if (useAnimator)
				this.animator.SetBool ("isStaying", false);
			Vector3 angle_vector = move_vector;
			angle_vector.y = 0;
			Quaternion characterTargetRotation = Quaternion.LookRotation (angle_vector);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, characterTargetRotation, 360 * 2 * Time.deltaTime);
		} else {
			if (useAnimator && !(mv_z != 0.0f && mv_x != 0.0f)){
				this.animator.SetBool ("isWalking", false);
				this.animator.SetBool ("isRunning", false);
				this.animator.SetBool ("isStaying", true);
			}
		}
	}

	public bool CheckGrounded()
	{
		//CharacterControlle.IsGroundedがtrueならRaycastを使わずに判定終了
		if (characterController.isGrounded) {
			return true;
		} else {
			return false;
		}
		//放つ光線の初期位置と姿勢
		//若干身体にめり込ませた位置から発射しないと正しく判定できない時がある
		Ray ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
		//Raycastがhitするかどうかで判定
		//地面にのみ衝突するようにレイヤを指定する
		return Physics.Raycast(ray, tolerance,1);
	}
	
	private void move(){
		moveGravity ();
		if (flowchart.GetBooleanVariable ("StopOther") == false) {
			moveInit ();
			moveJump ();
			moveWalking ();
			characterController.Move (move_vector * Time.deltaTime);
		} else {
			this.animator.SetBool ("isJump",false);
			this.animator.SetBool ("isWalking", false);
			this.animator.SetBool ("isRunning", false);
			this.animator.SetBool ("isStaying", true);
		}
	}

	public void setAdjustSpeed(float value){
		this.AdjustSpeed = value;
	}

	public void setJumpPower(float value){
		this.JumpPower = value;
	}

	public void setScale(float value){
		this.transform.localScale = new Vector3 (value,value,value);
	}


}
