using UnityEngine;
using System.Collections;

public class SunScript : MonoBehaviour {

	[SerializeField]
	public float angle = 0.0f;

	private float speed = 2.0f;

	private new Light light ;

	private float maxIntensity = 2.0f;

	public bool isMove = true;

	// Use this for initialization
	void Start () {
		light = this.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (isMove) {

			angle += speed * Time.deltaTime;

			if (angle > 360.0f)
				angle -= 360.0f;

			light.intensity = maxIntensity * (Mathf.Sin (angle * Mathf.Deg2Rad));

			//float new_y = distance * Mathf.Cos (angle);
			//float new_z = distance * Mathf.Sin (angle);

			//this.transform.localPosition = new Vector3 ( -300.0f,new_y,new_z );

			//this.transform.LookAt (central);
			this.transform.localRotation = Quaternion.Euler (new Vector3 (angle, 0, 0));

		}
	}

	public void setSunSpeed(float value){
		this.speed = value;
	}
	public void setMovable(bool value){
		this.isMove = value;
	}

}
