using UnityEngine;
using System.Collections;

public class MapCameraScript : MonoBehaviour {

	[SerializeField]
	public GameObject target;
	public float tall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = target.transform.position;
		newPos.y = tall;
		Vector3 newRotation = target.transform.rotation.eulerAngles;
		newRotation.x = 90.0f;
		transform.position = newPos;
		transform.rotation = Quaternion.Euler (newRotation);
	}
}
