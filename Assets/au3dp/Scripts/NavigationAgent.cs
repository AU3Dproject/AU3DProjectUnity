using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationAgent : MonoBehaviour {

	public float tall = 1.0f;
	public GameObject fromTarget;
	public GameObject toTarget;

	private UnityEngine.AI.NavMeshAgent agent;
	private UnityEngine.AI.NavMeshPath path;
	public Material line_material;
	public Material node_material;

	private List<GameObject> line_list = new List<GameObject>();

	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		//line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		if (agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete) {
			agent.Warp(PlayerManager.Instance.player_model.transform.position);
		}

		if (fromTarget != null && toTarget != null && agent != null && agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid) {

			//line.enabled = true;

			agent.SetDestination(toTarget.transform.position);

			path = new UnityEngine.AI.NavMeshPath();
			agent.CalculatePath(toTarget.transform.position, path);

			//line.numPositions = path.corners.Length;
			//line.SetVertexCount(path.corners.Length);

			Vector3[] destination = path.corners;

			for (int i = 0; i < destination.Length; i++) {
				destination[i] = new Vector3(destination[i].x, destination[i].y + tall, destination[i].z);
			}

			removeLine();
			drawNode(destination[0]);
			for (int i = 0; i < destination.Length-1; i++) {
				drawLine(destination[i], destination[i + 1]);
			}
			

		}

		if (toTarget == null) {
			removeLine();
		}
	}

	private void drawLine(Vector3 from, Vector3 to) {
		GameObject line_renderer_object = new GameObject("lineRenderer", typeof(LineRenderer));
		LineRenderer line_renderer = line_renderer_object.GetComponent<LineRenderer>();
		line_renderer.SetPositions(new Vector3[] { from, to });
		line_renderer.material = line_material;
		line_renderer_object.transform.parent = transform;
	}

	private void drawNode(Vector3 start) {
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		go.GetComponent<SphereCollider>().enabled = false;
		go.GetComponent<MeshRenderer>().material = node_material;
		go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		go.transform.position = start;
		go.transform.parent = transform;
	}

	private void removeLine() {
		if (transform.childCount > 0) {
			foreach (Transform child in transform) {
				Destroy(child.gameObject);
			}
		}
	}

	public void warpAgent() {
		agent.Warp(transform.position);
	}
	public void warpAgent(Vector3 pos) {
		agent.Warp(pos);
	}


}
