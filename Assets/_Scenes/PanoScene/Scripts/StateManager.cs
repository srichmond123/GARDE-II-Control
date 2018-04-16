using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 StateManager is an object used for accessing and setting the state of the program 
 across the various scripts that manipulate the state.

 Currently it is being implemented as a bunch of fields with getters and setters,
 but eventually, it would be nice to have an object (like in js) containing them.
     */

public class StateManager : MonoBehaviour {

    private GameObject selected = null; // The current tag being selected
    private GameObject falcon; // The instance of the falcon controller
    private GameObject falconCursor; // The cursor being manipulated by the falcon

    private Camera falconCamera;

    private Vector3 cursorPosition; // The position of the cursor every frame

    private bool[] falconButtons;

    private int buttons; // bit 1 is the pan button, bit 2 is the click button, bit 3 is the top button of the falcon

    public GameObject getSelected()
    {
        return selected;
    }

    public void setSelected(GameObject g)
    {
        selected = g;
    }

    public Vector3 getCursorPosition()
    {
        return this.cursorPosition;
    }

    public int getButtons()
    {
        return buttons;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        this.falcon = GameObject.Find("Falcon");
        if (this.falcon)
        {
            this.falconCursor = GameObject.Find("CursorSphere");
            this.falconCamera = GameObject.Find("CursorCamera").GetComponent<Camera>();
            falconButtons = new bool[] { false, false, false, false };
        }
    }

    private void Update()
    {
        buttons = 0;
        if (this.falcon) // Update the cursor position every frame
        {
            this.cursorPosition = falconCamera.WorldToScreenPoint(this.falconCursor.transform.position);
            FalconUnity.getFalconButtonStates(0, out falconButtons);
            buttons |= falconButtons[0] ? 1 : 0; // middle button
            buttons |= falconButtons[1] ? 2 : 0; // left button
            buttons |= falconButtons[2] ? 4 : 0; // top button
            buttons |= falconButtons[3] ? 2 : 0; // right button
        }
        else
        {
            this.cursorPosition = Input.mousePosition;
            buttons |= Input.GetMouseButton(1) ? 1 : 0; // right mouse button
            buttons |= Input.GetMouseButton(0) ? 2 : 0; // left mouse button
        }
    }


}
