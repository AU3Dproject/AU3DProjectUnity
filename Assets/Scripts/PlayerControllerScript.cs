using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {

	[SerializeField]
	//Playerの動作の許可フラグ
	public static bool activeFlag;
	//Playerオブジェクト
	public GameObject Player;
	//Cameraオブジェクト
	public GameObject Camera;
	
	void Start () {
		activeFlag = true;
	}

}
