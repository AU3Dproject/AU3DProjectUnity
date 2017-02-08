using UnityEngine;

public class NavigationDetail : MonoBehaviour {

	[SerializeField,Header("この場所の名称")]
	public string NavigationName = "この場所の名称";
	[SerializeField, Header("この場所の詳細メッセージ")]
	public string NavigationDescription = "この場所の詳細";
	[SerializeField, Header("サムネイル画像のURL（Resourcesディレクトリ配下）")]
	public string ThumbnailAddr = "Image/NewImage";
	
}
