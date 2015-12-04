using UnityEngine;
using System.Collections;
using Fungus;

public class AIScript : MonoBehaviour {

	//目的地
	public Vector3 destination;
	//動作状況
	public bool move = false;
	//目的地決定の範囲
	public float moveRange = 5.0f;
	//移動タイミングの範囲
	public float timeRange = 5.0f;
	//移動タイミングのための時間
	private float time = 0.0f;
	//次の移動タイミング
	private float nextTime = 5.0f;
	//AIで最重要なNavMeshAgent
	private NavMeshAgent agent = null;
	//AnimatorComponent
	private Animator animator = null;


	/* Start
	 * 　（１）初期化
	 */
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		destination = new Vector3 (0.0f, 0.0f, 0.0f);
		agent.angularSpeed = 300;
	}
	
	/* FixedUpdate
	 * 　（１）初期化
	 */
	void FixedUpdate () {

		//時間計測
		time += Time.deltaTime;
		//時間が移動タイミングに達したら
		if (nextTime <= time) {
			//Playerが動作している時に
			if(PlayerControllerScript.activeFlag!=false){
				//目的地ランダム決定　時間初期化　次時間ランダム決定
				setDestinationRandom();
				agent.SetDestination(destination);
				time = 0.0f;
				nextTime = Random.value * timeRange;
			}
		}
		//要修正！！！
		//Playerが非動作の時は現地点を目的地とする
		if (PlayerControllerScript.activeFlag!=true) {
			agent.SetDestination(this.transform.position);
		}

		//行き先が無いなら立ち止まるアニメーション
		if (agent.hasPath==false) {
			animator.SetBool("isStaying",true);
			animator.SetBool ("isWalking",false);
		//行き先が在るのなら歩くのじゃ
		} else {
			animator.SetBool ("isStaying", false);
			animator.SetBool ("isWalking", true);
		}

	}

	/* 目的地のランダム設定
	 * 　（１）次の目的地を移動範囲内で決定する。
	 */
	void setDestinationRandom(){
		Vector3 pos = this.transform.position;
		float new_x = pos.x + (0.5f - Random.value) * moveRange;
		float new_y = 1.0f;
		float new_z = pos.z + (0.5f - Random.value) * moveRange;
		destination = new Vector3 (new_x,new_y,new_z);
	}
}
