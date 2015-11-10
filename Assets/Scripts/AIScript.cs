using UnityEngine;
using System.Collections;
using Fungus;

public class AIScript : MonoBehaviour {

	public Vector3 destination;
	public bool move = false;
	public float moveRange = 5.0f;
	public float timeRange = 5.0f;
	public Flowchart flowchart;

	private float time = 0.0f;
	private float nextTime = 5.0f;
	private NavMeshAgent agent = null;
	private Animator animator = null;


	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		destination = new Vector3 (0.0f, 0.0f, 0.0f);
		agent.angularSpeed = 300;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		time += Time.deltaTime;
		if (nextTime <= time) {
			if(flowchart.GetBooleanVariable("StopOther")==false){
				setDestinationRandom();
				agent.SetDestination(destination);
				time = 0.0f;
				nextTime = Random.value * timeRange;
			}
		}
		if (flowchart.GetBooleanVariable("StopOther")==true) {
			agent.SetDestination(this.transform.position);
		}

		if (agent.hasPath==false) {
			animator.SetBool("isStaying",true);
			animator.SetBool ("isWalking",false);
		} else {
			animator.SetBool ("isStaying", false);
			animator.SetBool ("isWalking", true);
		}

	}

	void setDestinationRandom(){
		Vector3 pos = this.transform.position;
		float new_x = pos.x + (0.5f - Random.value) * moveRange;
		float new_y = 1.0f;
		float new_z = pos.z + (0.5f - Random.value) * moveRange;
		destination = new Vector3 (new_x,new_y,new_z);
	}
}
