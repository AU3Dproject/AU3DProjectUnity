using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconRotation : MonoBehaviour {

    RectTransform rectTransform;

    [SerializeField]
    public Vector3 newAngle = new Vector3(0, 1, 0);
    public Button button;

    bool isRotate = false;

    // Use this for initialization
    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        if (isRotate) {
            rectTransform.Rotate(newAngle);
        } else {
            rectTransform.Rotate(Vector3.zero);
            rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void setRotate(bool isRotate) {
        this.isRotate = isRotate;
    }
}
