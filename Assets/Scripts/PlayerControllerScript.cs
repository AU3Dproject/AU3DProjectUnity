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

    private CharacterMove player_move;
    private CameraMove camera_move;

	/* Start
	 * 　（１）初期化
	 */
	void Start () {
        player_move = Player.GetComponent<CharacterMove>();
        camera_move = Camera.GetComponent<CameraMove>();
		activeFlag = true;
	}

    /* Update
	 * 　（１）何もしーひん
	 */
    void Update() {
        /*
        if (activeFlag) {
            player_move.enabled = true;
            camera_move.enabled = true;
        }else {
            player_move.enabled = false;
            camera_move.enabled = false;
        }*/
	}

}
