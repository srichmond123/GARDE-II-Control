﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkenerScript : MonoBehaviour {
	// Use this for initialization
	public static float scrollPos = 0f;
	static Image image;
	static GameObject camera;

    float z = 0.2f;

    Vector3 falconPos;
    GameObject falcon;

	void Start () {
		image = gameObject.GetComponent<Image> ();
		camera = GameObject.Find("Main Camera");

        if (falcon = GameObject.Find("Tip"))
        {
            falconPos = Vector3.zero;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!falcon)
        {
            float mouseWheelDelta = Input.GetAxis("Mouse ScrollWheel"); //This is the change in mousewheel position, not absolute position
            scrollPos += mouseWheelDelta;
        }
        else
        {
            FalconUnity.getTipPosition(0, out falconPos);
            scrollPos = (float)Math.Tanh(z - (Mathf.Max(z, Mathf.Abs(falconPos.z))));  // * 0.5f;
        }


		//Image blackens completely at above 0.33 scrollPos or below -0.33, this shouldn't be too hard to translate/modify over to the falcon:
		image.color = new Color (0, 0, 0, Mathf.Abs(scrollPos) * 3);

		//This part doesn't need to change at all:
		ViewConeScript.modifyRotation (-camera.transform.localEulerAngles.y);

		//With the falcon, the part "Mathf.Abs(1.0f + scrollPos)" can just be changed to "falcon.z / 2.0f + 1.0" (ranges from 0.5f to 1.5f scale)
		ViewConeScript.modifyScale (1.0f + falconPos.z / 2.0f);
	}
}
