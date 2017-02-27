using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class MainMenuButtonScript : Button {

    Image buttonImage;

    // Update is called once per frame
    void Update() {

        if (buttonImage == null) {
            buttonImage = transform.FindChild("Image").gameObject.GetComponent<Image>();
        }
        
    }
}
