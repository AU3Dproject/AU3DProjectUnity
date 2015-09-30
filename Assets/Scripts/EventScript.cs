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
				if (Input.GetButtonDown ("MessageClick")) {
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
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(player.transform.position - this.transform.position), 0.1f);
		player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(this.transform.position - player.transform.position), 0.1f);
	}


}
