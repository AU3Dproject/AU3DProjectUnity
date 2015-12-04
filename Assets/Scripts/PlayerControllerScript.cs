using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {

	[SerializeField]
	//Playerの動作の許可フラグ
	public static bool activeFlag;
	//Playerのオブジェクト
	public GameObject Player;
	//Cameraのオブジェクト
	public GameObject Camera;

	/* Start
	 * 　（１）初期化
	 */
	void Start () {
		activeFlag = true;
	}
	
	/* Update
	 * 　（１）何もしーひん
	 */
	void Update () {
		//None
	}

}
