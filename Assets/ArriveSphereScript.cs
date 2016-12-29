using UnityEngine;
using System.Collections;

public class ArriveSphereScript : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if(GetComponent<MeshRenderer>().enabled == true)
			other.transform.parent.FindChild("NavigationAgent").GetComponent<NavigationAgent>().toTarget = null;
			GetComponent<MeshRenderer>().enabled = false;
		}
	}

}
