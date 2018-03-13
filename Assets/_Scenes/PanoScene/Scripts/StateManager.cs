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

    private Vector3 cursorPosition; // The position of the cursor every frame

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

    private void Start()
    {
        this.falcon = GameObject.Find("Falcon");
        if (this.falcon)
        {
            this.falconCursor = GameObject.Find("CursorSphere");
        }
    }

    private void Update()
    {
        if (this.falcon) // Update the cursor position every frame
        {
            this.cursorPosition = Camera.main.WorldToScreenPoint(this.falconCursor.transform.position);
        }
        else
        {
            this.cursorPosition = Input.mousePosition;
        }
    }


}
