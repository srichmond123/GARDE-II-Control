using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAction : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // OnClick code goes here ...
        // Currently, assuming the only thing being clicked is a tag
        Debug.Log(this.name); // Name of the object
    }
}