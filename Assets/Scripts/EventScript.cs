using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventScript : MonoBehaviour {

	[SerializeField]
	//Playerの接近時表示Object
	public MeshRenderer nearObject;
	//Playerの接近範囲
	public float nearDistance = 1.0f;
    //向き合わせを行うかどうか
    public bool isFace2face = true;
	//PlayerObject
	private GameObject player = null;

	private TalkEventScript eventScript = null;

	private bool isEventEnd = true;

	/* Start
	 * 　（１）初期化
	 */
	void Start () {
		nearObject.enabled = false;
		player = PlayerManager.Instance.player_model;
		eventScript = GetComponent<TalkEventScript>();
	}
	
	/* Update
	 * 　（１）接近時Objectの表示
	 * 　（２）Event開始ボタン押下でEvent開始
	 * 　（３）Event中はPlayerの動作停止
	 * 　（４）Event中にPlayerとNPCを向かい合わせる。
	 * 　（５）Event終了時にPlayerの動作開始
	 */
	void Update () {

		//Player接近時
		if (isAccess () && !eventScript.is_execute && PlayerManager.Instance.is_pause && isEventEnd) {

			//接近時Object表示
			if (nearObject != null) {
				nearObject.enabled = true;
			}
			//Event開始ボタン押下
			if(Input.GetButtonDown("Submit")){
				//イベントの開始とPlayer動作停止
				eventScript.is_execute = true;
				PlayerManager.Instance.is_pause = false;
				isEventEnd = false;
			}

		} else {
			//接近時Objectを消す
			if (nearObject != null) {
				nearObject.enabled = false;
			}
		}

		//イベント終了時にPlayerの動作を開始する
		if(!eventScript.is_execute && PlayerManager.Instance.is_pause == false && !isEventEnd){
			PlayerManager.Instance.is_pause = true;
			isEventEnd = true;
		}

		//イベント中はPlayerとNPCを向かい合わせる。
		if (eventScript.is_execute == true) {
			if (isFace2face)
				face2face();
		}

	}


	/* 接近時判定
	 * 　（１）PlayerとNPCの距離がnearDistance以下ならtrueを返す
	 * 　（２）そうじゃないならfalse
	 */
	private bool isAccess(){
		if (player == null)
			return false;
		float distance = Vector3.Distance (this.transform.position,player.transform.position);
		if (distance < nearDistance)
			return true;
		else
			return false;
	}


	/* 向かい合わせ
	 * 　（１）Playerの向きをNPCに向ける
	 * 　（２）NPCも同様
	 */
	private void face2face(){
		Vector3 to = new Vector3 (player.transform.position.x - transform.position.x,0, player.transform.position.z - transform.position.z);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(to), 0.1f);
		to = new Vector3 (transform.position.x - player.transform.position.x, 0, transform.position.z - player.transform.position.z);
		player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(to), 0.1f);
	}

}
