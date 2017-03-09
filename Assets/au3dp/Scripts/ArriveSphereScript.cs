using UnityEngine;
using System.Collections;

public class ArriveSphereScript : MonoBehaviour {


	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if(GetComponent<MeshRenderer>().enabled == true)
			NavigationManager.Instance.transform.FindChild("NavigationAgent").GetComponent<NavigationAgent>().toTarget = null;
			GetComponent<MeshRenderer>().enabled = false;
			transform.parent.FindChild("Quad").GetComponent<MeshRenderer>().enabled = false;
		}
	}

}
