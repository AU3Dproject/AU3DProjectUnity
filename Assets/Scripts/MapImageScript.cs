using UnityEngine;
using System.Collections;

public class MapImageScript : MonoBehaviour {

	[SerializeField]
	public bool isMovable = false;
	public float minZoomValue = 50;
	public float maxZoomValue = 260;
	public float zoomSpeed = 1.0f;

	public float moveSpeed = 5.0f;

    public bool isFlipHorizontal = true;
    public bool isFlipVertical = true;

	public Camera upMapCamera;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update() {
		if (isMovable) {
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

			mvx = Input.GetAxis("CameraHorizontal") * moveSpeed * (isFlipHorizontal ? -1 : 1);
			mvz = Input.GetAxis("CameraVertical") * moveSpeed * (isFlipVertical ? -1 : 1);
			upMapCamera.gameObject.transform.position += new Vector3(mvx, 0, mvz);
		}
	}

	public void setMovable(bool movable) {
		isMovable = movable;
	}

}
