using UnityEngine;
using System.Collections;

public class AllMeshCollider : MonoBehaviour {

	int i = 0;

	// Use this for initialization
	void Start () {
		addCollider(this.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void addCollider(Transform obj) {
		foreach(Transform o in obj) {
			if (o.tag == "NoCollider") continue;

			if (o.childCount > 0) addCollider(o);

			Debug.Log(o.name);

			MeshFilter mesh = o.GetComponent<MeshFilter>();
			if (mesh == null) continue;

			MeshCollider collider = o.GetComponent<MeshCollider>();
			if (collider == null) {
				collider = o.gameObject.AddComponent<MeshCollider>();
			}
			collider.sharedMesh = mesh.mesh;
		}
	}
}
