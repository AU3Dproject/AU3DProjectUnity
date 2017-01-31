using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconRotation : MonoBehaviour {

    RectTransform rectTransform;

    [SerializeField]
    public Vector3 update_angle = new Vector3(0, 1, 0);

    public bool isRotate = false;

    // Use this for initialization
    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        if (isRotate) {
            rectTransform.Rotate(update_angle);
        } else {
            rectTransform.Rotate(Vector3.zero);
            rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void setRotate(bool isRotate) {
        this.isRotate = isRotate;
    }
}
