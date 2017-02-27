using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : ManagerMonoBehaviour<NavigationManager> {

	public NavigationAgent agent = null;

	public void setToTarget(GameObject target) {
		if (agent != null) {
			agent.toTarget = target.transform.GetChild(1).gameObject;
		}
	}

}
