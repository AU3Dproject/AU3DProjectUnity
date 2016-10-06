using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventScript : MonoBehaviour {

	[SerializeField]
	//Playerの接近時表示Object
	public MeshRenderer nearObject;
	//Eventの開始Block
	public string blockName ="";
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
		player = (GameObject.Find("/PlayerController").GetComponent<PlayerControllerScript>()).Player;
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
		if (isAccess () && !eventScript.isExecute && PlayerControllerScript.activeFlag && isEventEnd) {

			//接近時Object表示
			nearObject.enabled = true;
			//Event開始ボタン押下
			if(Input.GetButtonDown("Submit")){
				//イベントの開始とPlayer動作停止
				eventScript.isExecute = true;
				PlayerControllerScript.activeFlag = false;
				isEventEnd = false;
			}

		} else {
			//接近時Objectを消す
			nearObject.enabled = false;
		}

		//イベント終了時にPlayerの動作を開始する
		if(!eventScript.isExecute && PlayerControllerScript.activeFlag==false && !isEventEnd){
			PlayerControllerScript.activeFlag=true;
			isEventEnd = true;
		}

		//イベント中はPlayerとNPCを向かい合わせる。
		if (eventScript.isExecute == true) {
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
		Vector3 to = new Vector3 (player.transform.position.x - this.transform.position.x,0, player.transform.position.z - this.transform.position.z);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(to), 0.1f);
		to = new Vector3 (this.transform.position.x - player.transform.position.x, 0, this.transform.position.z - player.transform.position.z);
		player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(to), 0.1f);
	}

}
