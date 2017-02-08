using UnityEngine;

public class AllMeshCollider : MonoBehaviour {

	[Header("このスクリプトを付与したオブジェクトの子すべてに対しMeshColliderを付与する。")]
	[SerializeField,Header("↓この文字列のタグに対してはコライダーを付与しない。")]
	public string no_collider_tag = "NoCollider";
	
	void Awake () {
		addCollider(transform);
	}

	private void addCollider(Transform parent) {
		foreach(Transform child in parent) {
			if (child.tag == no_collider_tag) continue;

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
