using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : ManagerMonoBehaviour<SystemManager> {

	[SerializeField]
	public TimeManager timeManager;
	public TalkEventValiableManager valiableManager;
	public NavigationManager navigationManager;
	//public MapManager mapManager;
	
}
