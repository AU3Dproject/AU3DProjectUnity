using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaitArrowScript : MonoBehaviour {

	[SerializeField]
	public float fadeSpeed = 2.0f;
	private int fadein = 1;
	public bool isVisible = true;
	public bool isFade = true;

	private Image arrowImage = null;

	// Use this for initialization
	void Start () {
		arrowImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

		if (isVisible) {

			if (isFade) {

				float new_a = ((Time.deltaTime / fadeSpeed)) * fadein;

				arrowImage.color += new Color(0,0,0, new_a);

				if (arrowImage.color.a >= 1) {
					arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, 1);
					fadein = -1;
				}
				if (arrowImage.color.a <= 0) {
					arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, 0);
					fadein = 1;
				}

			} else {
				arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, 1);
			}

		} else {
			arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, 0);
		}
	}

	public void setVisible(bool visible) {
		if (visible) {
			isVisible = visible;
			arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, 1);
		} else {
			isVisible = visible;
			arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, 0);
		}
	}
}
