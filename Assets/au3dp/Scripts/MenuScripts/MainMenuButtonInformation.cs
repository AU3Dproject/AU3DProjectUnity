using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MainMenuButton))]
public class MainMenuButtonInformation : MonoBehaviour {

	[SerializeField]

	[Header("ボタンの基本情報")]
	public string title = "タイトル";
	public string description = "解説";

}

