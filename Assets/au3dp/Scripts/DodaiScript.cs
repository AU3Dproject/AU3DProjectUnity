using UnityEngine;
using System.Collections;

public class DodaiScript : MonoBehaviour {

	[SerializeField]

	private Vector3 init_pos = new Vector3(0.0f,0.0f,0.0f);

	public Vector3 pos_speed = new Vector3(0.0f,0.0f,0.0f);
	public Vector3 rot_speed = new Vector3(0.0f,0.0f,0.0f);
	public Vector3 sca_speed = new Vector3(0.0f,0.0f,0.0f);

	public Vector3 pos_max = new Vector3(0.0f,0.0f,0.0f);
	public Vector3 pos_min = new Vector3(0.0f,0.0f,0.0f);

	public Vector3 rot_max = new Vector3(0.0f,0.0f,0.0f);
	public Vector3 rot_min = new Vector3(0.0f,0.0f,0.0f);

	public Vector3 sca_max = new Vector3(0.0f,0.0f,0.0f);
	public Vector3 sca_min = new Vector3(0.0f,0.0f,0.0f);






	// Use this for initialization
	void Start () {
		init_pos = this.transform.position;
		
	}
	
	// Update is called once per frame
	void FUpdate () {
	
		Vector3 next_pos = this.transform.position;

		//X座標
		next_pos.x += pos_speed.x * Time.fixedDeltaTime;
		if (next_pos.x > pos_max.x)next_pos.x = pos_max.x;

		//Y座標
		next_pos.y += pos_speed.y * Time.fixedDeltaTime;
		if (next_pos.y > pos_max.y)next_pos.y = pos_max.y;

		//Z座標
		next_pos.z += pos_speed.z * Time.fixedDeltaTime;
		if (next_pos.z > pos_max.z)next_pos.z = pos_max.z;



		this.transform.position = next_pos;


	}
}
