using UnityEngine;
using System.Collections;

public class NavigationAgent : MonoBehaviour {

	public float tall = 1.0f;
	public GameObject fromTarget;
	public GameObject toTarget;

	private NavMeshAgent agent;
	private NavMeshPath path;
	public LineRenderer line;

	public Material startLineMaterial;
	public Material endLineMaterial;
	public Material normalLineMaterial;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		//line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (fromTarget != null && toTarget != null && agent != null && line != null && agent.pathStatus != NavMeshPathStatus.PathInvalid) {
			transform.position = new Vector3(fromTarget.transform.position.x, transform.position.y, fromTarget.transform.position.z);
			agent.Warp(transform.position);
			transform.rotation = fromTarget.transform.rotation;

			path = new NavMeshPath();
			agent.CalculatePath(toTarget.transform.position, path);

			line.SetVertexCount(path.corners.Length);

			Vector3[] destination = path.corners;

			for (int i=0;i<destination.Length;i++) {
				destination[i] = new Vector3(destination[i].x,destination[i].y+tall,destination[i].z);
			}

			line.SetPositions(destination);

			//for (int i = 0; i < line.materials.Length; i++) {
			//	if (i == 0) {
			//		line.materials[i] = startLineMaterial;
			//	} else if (i == line.materials.Length - 1) {
			//		line.materials[i] = endLineMaterial;
			//	} else {
			//		line.materials[i] = normalLineMaterial;
			//	}
			//}

		}
	}

	public void warpAgent() {
		agent.Warp(transform.position);
	}
	public void warpAgent(Vector3 pos) {
		agent.Warp(pos);
	}


}
