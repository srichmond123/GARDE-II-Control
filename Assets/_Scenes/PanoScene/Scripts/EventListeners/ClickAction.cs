using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAction : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // OnClick code goes here ...
        GameObject objectClicked = eventData.pointerCurrentRaycast.gameObject; // get the object that was pressed

        if (objectClicked.tag == "Tag") // A tag was pressed
        {
            Debug.Log(objectClicked.name); // Name of the object
        }
        else if (objectClicked.tag == "Bin") // The bin was pressed, so we move the tag to the bin
        {
            Debug.Log("Bin Clicked");
        }
        else if (objectClicked.tag == "Image") // The image area was pressed, so here we cast a tag onto the sphere
        {
            Debug.Log("Image Clicked");
        }
    }
}