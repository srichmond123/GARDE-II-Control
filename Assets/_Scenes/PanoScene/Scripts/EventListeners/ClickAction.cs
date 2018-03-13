using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAction : MonoBehaviour, IPointerClickHandler
{
    StateManager state; // State of the application

    private GameObject tagPrefab;
    public Material tagMaterial;

    public void Start()
    {
        state = GameObject.Find("Canvas").GetComponent<StateManager>();
        tagPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tagPrefab.transform.localScale = Vector3.zero;
    }

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
            if (true) // TODO: Check if a tag is currently selected
            {
                Vector3 cursorPosition = state.getCursorPosition(); // Use the cursor position to cast a ray onto the sphere
                Ray ray = Camera.main.ScreenPointToRay(cursorPosition);  // The ray that will be casted onto the sphere

                // In the following two lines, since the sphere collider is outside the sphere
                // We move the point of the ray well outside of the sphere, then invert the direction
                // This way, we cast ray to the same point of the sphere, but from the outside rather than the inside
                ray.origin = ray.GetPoint(100);
                ray.direction = -ray.direction;

                RaycastHit hit; // The raycast

                Debug.DrawRay(ray.origin, ray.direction, Color.red, 5);
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject newObject = Instantiate(tagPrefab, hit.point * 0.95f, Quaternion.identity); // Create the new object using the tagPrefab
                    newObject.transform.LookAt(Vector3.zero); // Make it face the center of the sphere
                    newObject.transform.localScale = new Vector3(0.2f, 0.1f, 0.00001f);
                    newObject.name = "Some Name"; // CHANGE THIS LATER
                    Debug.Log(hit.point);

                    // ---- Below is old code used to create the tag whereever the click happened. It isn't being used now but may be useful later
                    // --------------------------------------------------------------------------------------------------------------------------
                    //GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.tag, hit.point * 0.95f, Quaternion.identity);
                    //gameObject.transform.LookAt(Vector3.zero);
                    //gameObject.name = "Tag " + this.tags.Count;
                    //gameObject.transform.localScale = new Vector3(20f, 5f, 1f);
                    //this.tag.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
                    //gameObject.AddComponent(typeof(Tag));
                    //gameObject.transform.parent = this.tagContainer.transform;
                    //this.tags.Add(gameObject);
                    //this.keycam.transform.position = Vector3.zero;
                    //this.keycam.transform.LookAt(this.ray.point);
                    //this.keycam.transform.position = Vector3.MoveTowards(this.keycam.transform.position, this.ray.point, Vector3.Distance(this.keycam.transform.position, this.ray.point) * 0.8f);
                    //this.startTag(gameObject);
                }
            }
        }
    }
}