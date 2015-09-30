using UnityEngine;
using System.Collections;
using Fungus;

public class EventScript : MonoBehaviour {

	[SerializeField]
	public Flowchart flowchart ;
	public GameObject player;
	public MeshRenderer nearObject;

	public string blockName ;
	public float nearDistance = 1.0f;

	private bool isMessageActive = false;


	// Use this for initialization
	void Start () {
		nearObject.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAccess ()) {
			nearObject.enabled = true;
			if(flowchart.GetBooleanVariable("StopOther")==true){
				face2face();
			}else{
				if (Input.GetButtonDown ("Submit")) {
					isMessageActive = true;
					flowchart.ExecuteBlock (blockName);
				}
			}
		} else {
			nearObject.enabled = false;
			isMessageActive=false;
		}
	}

	private bool isAccess(){
		float distance = Vector3.Distance (this.transform.position,player.transform.position);
		if (distance < nearDistance)
			return true;
		else
			return false;
	}

	private void face2face(){
		Vector3 to = new Vector3 (player.transform.position.x - this.transform.position.x,0, player.transform.position.z - this.transform.position.z);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(to), 0.1f);
		to = new Vector3 (this.transform.position.x - player.transform.position.x, 0, this.transform.position.z - player.transform.position.z);
		player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(to), 0.1f);
	}


}
