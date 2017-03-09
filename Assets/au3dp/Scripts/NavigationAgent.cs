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
	}
	
	// Update is called once per frame
	void Update () {

		//移動先までの道のりが確定できていたら常に開始点をプレイヤーに移動させる。
		if (agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete) {
			agent.Warp(PlayerManager.Instance.player_model.transform.position);
		}

		//各種条件が整ったときにルートを求め表示する。
		if (fromTarget != null && toTarget != null && agent != null && agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid) {

			//行き先設定
			agent.SetDestination(toTarget.transform.position);

			//ルートを計算
			path = new UnityEngine.AI.NavMeshPath();
			agent.CalculatePath(toTarget.transform.position, path);

			//配列としてルートの各点を取得
			Vector3[] destination = path.corners;

			//ルートに高さを加算。
			for (int i = 0; i < destination.Length; i++) {
				destination[i] += tall * Vector3.up;
			}

			//ルート表示を一旦削除
			removeLine();

			//ルートを表示する
			//drawNode(destination[0]);
			for (int i = 0; i < destination.Length-1; i++) {
				drawLine(destination[i], destination[i + 1]);
				drawNode(destination[i]);
			}
			

		}

		//行き先がなければルート表示を削除
		if (toTarget == null) {
			removeLine();
		}
	}

	/// <summary>
	/// LineRendererを生成し、fromからtoまで線を描画する。
	/// </summary>
	/// <param name="from">描画線の開始位置</param>
	/// <param name="to">描画線の終端位置</param>
	private void drawLine(Vector3 from, Vector3 to) {
		GameObject line_renderer_object = new GameObject("lineRenderer", typeof(LineRenderer));
		LineRenderer line_renderer = line_renderer_object.GetComponent<LineRenderer>();
		line_renderer.SetPositions(new Vector3[] { from, to });
		line_renderer.material = line_material;
		line_renderer_object.transform.parent = transform;
	}

	/// <summary>
	/// ノードの描画
	/// </summary>
	/// <param name="position">描画するノードの位置</param>
	private void drawNode(Vector3 position) {
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		go.GetComponent<SphereCollider>().enabled = false;
		go.GetComponent<MeshRenderer>().material = node_material;
		go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		go.transform.position = position;
		go.transform.parent = transform;
	}

	/// <summary>
	/// 描画した線やノードの削除（子オブジェクトの全消去）
	/// </summary>
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
