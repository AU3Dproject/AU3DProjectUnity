using UnityEngine;
using System.Collections;

public class MapImageScript : MonoBehaviour {

	[SerializeField]
	public float minZoomValue = 50;
	public float maxZoomValue = 260;
	public float zoomSpeed = 1.0f;

	public float moveSpeed = 5.0f;


	
	public Camera upMapCamera;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKey(KeyCode.Q)) {
			upMapCamera.orthographicSize += zoomSpeed * Time.deltaTime;
			if (upMapCamera.orthographicSize > maxZoomValue) {
				upMapCamera.orthographicSize = maxZoomValue;
			}
		}
		if (Input.GetKey(KeyCode.E)) {
			upMapCamera.orthographicSize -= zoomSpeed * Time.deltaTime;
			if (upMapCamera.orthographicSize < minZoomValue) {
				upMapCamera.orthographicSize = minZoomValue;
			}
		}

		float mvx = 0.0f;
		float mvz = 0.0f;

		mvx = Input.GetAxis("CameraHorizontal") * moveSpeed;
		mvz = Input.GetAxis("CameraVertical") * moveSpeed;
		upMapCamera.gameObject.transform.position += new Vector3(mvx, 0, mvz);
	}
}
