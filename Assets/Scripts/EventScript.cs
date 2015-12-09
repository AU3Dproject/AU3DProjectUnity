using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Fungus;

public class EventScript : MonoBehaviour {

	[SerializeField]
	//イベントのflowchart
	public Flowchart flowchart = null;
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

	private bool isEventEnd = true;

	/* Start
	 * 　（１）初期化
	 */
	void Start () {
		flowchart = (this.transform.FindChild("Flowchart")).GetComponent<Flowchart>();
		nearObject.enabled = false;
		player = (GameObject.Find("/PlayerController").GetComponent<PlayerControllerScript>()).Player;
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
		if (isAccess ()) {

			//接近時Object表示
			nearObject.enabled = true;
			//Event開始ボタン押下
			if(Input.GetButtonDown("Submit")){
				//イベントの開始とPlayer動作停止
				if(blockName!="" && flowchart!=null && isFlowchartActive()==false) {
					flowchart.ExecuteBlock(blockName);
					PlayerControllerScript.activeFlag=false;
					if(isEventEnd==true)isEventEnd=false;
				}
			}
			//イベント中はPlayerとNPCを向かい合わせる。
			if(this.isFlowchartActive() == true){
				if(isFace2face) face2face ();
			}

		} else {
			//接近時Objectを消す
			nearObject.enabled = false;
		}

		//イベント終了時にPlayerの動作を開始する
		if(!this.isFlowchartActive() && PlayerControllerScript.activeFlag==false&&isEventEnd==false){
			PlayerControllerScript.activeFlag=true;
			isEventEnd=true;
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


	/* Event実行判定
	 * 　（１）Flowchartの各Blockを取得し実行中か判定する。
	 * 　（２）実行中ならTrue
	 * 　（３）ちなみにFlowchart設定ミスやBlock無しFlowchartの場合エラー出るんで
	 */
	private bool isFlowchartActive(){

        if (GameObject.Find("/MenuDialog") != null) return true;

		Block [] blocks = flowchart.transform.GetComponents<Block>();
		if (blocks != null) {
			foreach (Block block in blocks) {
				if (block.IsExecuting ()){
					return true;
				}else{
					continue;
				}
			}
		}
		return false;
	}

}
