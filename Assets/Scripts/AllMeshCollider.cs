using UnityEngine;

public class AllMeshCollider : MonoBehaviour {

	[SerializeField,Header("aaa")]
	public string a;
	
	void Awake () {
		addCollider(transform);
	}

	private void addCollider(Transform parent) {
		foreach(Transform child in parent) {
			if (child.tag == "NoCollider") continue;

			if (child.childCount > 0) addCollider(child);

			MeshFilter mesh = child.GetComponent<MeshFilter>();
			if (mesh == null) continue;

			MeshCollider collider = child.GetComponent<MeshCollider>();
			if (collider == null) {
				collider = child.gameObject.AddComponent<MeshCollider>();
			}
			collider.sharedMesh = mesh.mesh;
		}
	}
}
