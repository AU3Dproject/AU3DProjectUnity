using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	
	[SerializeField]
	GameObject Player = null;
	
	[SerializeField]
	float distance = 1.2f ;
	
	[SerializeField]
	float HorizontalAngle = Mathf.PI ;

	[SerializeField]
	float VerticalAngle = 0.0f ;

	[SerializeField]
	float tall = 1.0f;
	
	[SerializeField]
	float angle_speed = 2.0f;

	[SerializeField]
	float zoom = 0.0f;

	[SerializeField]
	bool isTP = true;

	[SerializeField]
	SkinnedMeshRenderer playerModel = null;

	private Camera cam = null;
	private RaycastHit hit;
	private int hit_num = 0;
	
	// Use this for initialization
	public void Start () {
		cam = GetComponent<Camera> ();
	}

	
	// Update is called once per frame
	public void FixedUpdate () {
		if (isTP == true) {
			UpdateTP ();
		} else {
			UpdateFP ();
		}
	}


	//三人称視点の更新
	private void UpdateTP(){
		this.zoom += Input.GetAxis ("Zoom") * 0.1f;
		this.HorizontalAngle += Mathf.Deg2Rad * angle_speed * Input.GetAxisRaw ("Horizontal2");
		this.VerticalAngle += Mathf.Deg2Rad * angle_speed * Input.GetAxisRaw ("Vertical2");
		
		if (HorizontalAngle >= Mathf.PI * 2.0f)
			HorizontalAngle -= Mathf.PI * 2.0f;
		
		if (VerticalAngle >= 1.3f)
			VerticalAngle = 1.3f;
		
		if (VerticalAngle <= -0.85f)
			VerticalAngle = -0.85f;
		
		if (zoom <= -distance / 2)
			zoom = -distance / 2;
		
		if (zoom >= distance * 2)
			zoom = distance * 2;
		
		float new_x = - (distance + zoom) * Mathf.Sin (HorizontalAngle) - Mathf.Cos (VerticalAngle) * Mathf.Sin (HorizontalAngle) + Player.transform.position.x;
		float new_y = (distance + zoom) * Mathf.Sin (VerticalAngle) + (Player.transform.position.y + tall);
		float new_z = - (distance + zoom) * Mathf.Cos (HorizontalAngle) - Mathf.Cos (VerticalAngle) * Mathf.Cos (HorizontalAngle) + Player.transform.position.z;
		
		this.transform.position = new Vector3 (new_x, new_y, new_z);
		this.transform.LookAt (Player.transform.position + new Vector3 (0.0f, tall, 0.0f));

		Vector3 ptv = Player.transform.position + new Vector3 (0, tall, 0);
		Vector3 normal = (this.transform.position - ptv).normalized;
		if (Physics.Raycast (ptv, normal, out hit,distance + zoom,1)) {
			//Debug.Log ((hit_num++)+":Hit : "+hit.collider.name);
			this.transform.position = hit.point;
		}

	}

	//一人称視点の更新
	private void UpdateFP(){
		this.HorizontalAngle += Mathf.Deg2Rad * angle_speed * Input.GetAxisRaw ("Horizontal2");
		this.VerticalAngle += angle_speed * Input.GetAxisRaw ("Vertical2");
		
		if (HorizontalAngle >= Mathf.PI * 2.0f)
			HorizontalAngle -= Mathf.PI * 2.0f;
		
		if (VerticalAngle >= 80.0f)
			VerticalAngle = 80.0f;
		
		if (VerticalAngle <= -63.0f)
			VerticalAngle = -63.0f;
		
		float new_x = - (distance) * Mathf.Sin (HorizontalAngle) + Player.transform.position.x;
		float new_y = tall + Player.transform.position.y;
		float new_z = - (distance) * Mathf.Cos (HorizontalAngle) + Player.transform.position.z;
		
		Vector3 lookVec = Player.transform.position + new Vector3(0.0f,tall,0.0f);
		
		this.transform.position = new Vector3 (new_x, new_y, new_z);
		this.transform.LookAt (lookVec);
		
		float new_rotation_x = this.transform.localRotation.eulerAngles.x + VerticalAngle;
		float new_rotation_y = this.transform.localRotation.eulerAngles.y;
		float new_rotation_z = this.transform.localRotation.eulerAngles.z;
		
		this.transform.localRotation = Quaternion.Euler(new Vector3(-new_rotation_x,new_rotation_y,new_rotation_z));
	}







	public void setAngleSpeed(float value){
		angle_speed = value;
	}

	public void setTall(float value){
		tall = value;
		distance = value;
	}

	public void setTPMode(bool value){
		isTP=!value;
		if (!isTP) {
			distance = 0.1f;
			tall = 1.2f;
			if(playerModel != null){
				playerModel.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
			}
		} else {
			distance = 1.2f;
			tall = 1.0f;
			if(playerModel != null){
				playerModel.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			}
		}
	}

	
}
