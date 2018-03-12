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

    private GameObject selected = null;

    public GameObject getSelected()
    {
        return selected;
    }

    public void setSelected(GameObject g)
    {
        selected = g;
    }
}
