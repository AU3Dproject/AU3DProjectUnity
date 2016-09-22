using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {

    [SerializeField]
    public string text = "";
    public bool isTextVisible;
    public float textVisibleFrame = 30.0f;

    private float time;
    private Text textComponent;

	// Use this for initialization
	void Start () {
        textComponent = transform.FindChild("Text").GetComponent <Text> ();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        time += Time.fixedDeltaTime;

        if(time % textVisibleFrame > textVisibleFrame) {

        }
	}
}
