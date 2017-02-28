using UnityEngine;
using System.Collections;

public class NavigationAgent : MonoBehaviour {

	public float tall = 1.0f;
	public GameObject fromTarget;
	public GameObject toTarget;

	private UnityEngine.AI.NavMeshAgent agent;
	private UnityEngine.AI.NavMeshPath path;
	public LineRenderer line;

	public Material startLineMaterial;
	public Material endLineMaterial;
	public Material normalLineMaterial;

	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		//line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		//agent.Warp(PlayerManager.Instance.transform.position);

		if (fromTarget != null && toTarget != null && agent != null && line != null && agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid && line.enabled==false) {

			line.enabled = true;

			agent.SetDestination(toTarget.transform.position);

			path = new UnityEngine.AI.NavMeshPath();
			agent.CalculatePath(toTarget.transform.position, path);

			line.numPositions = path.corners.Length;
			//line.SetVertexCount(path.corners.Length);

			Vector3[] destination = path.corners;

			//for (int i=0;i<destination.Length;i++) {
			//	destination[i] = new Vector3(destination[i].x,destination[i].y+tall,destination[i].z);
			//}

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

		if (toTarget == null) {
			line.enabled = false;
		}
	}

	public void warpAgent() {
		agent.Warp(transform.position);
	}
	public void warpAgent(Vector3 pos) {
		agent.Warp(pos);
	}


}
