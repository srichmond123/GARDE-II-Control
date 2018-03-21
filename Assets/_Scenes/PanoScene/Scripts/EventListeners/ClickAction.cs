using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAction : MonoBehaviour, IPointerClickHandler
{
    StateManager state; // State of the application

    private GameObject tagPrefab;
    private GameObject sphere;

    public Material tagMaterial;

    public void Start()
    {
        state = GameObject.Find("Canvas").GetComponent<StateManager>();

        tagPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
		tagPrefab.name = "TagPrefab"; //So it can be destroyed
        tagPrefab.transform.localScale = Vector3.zero;

        sphere = GameObject.Find("TagSphere");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // OnClick code goes here ...
        GameObject objectClicked = eventData.pointerCurrentRaycast.gameObject; // get the object that was pressed

        if (eventData.button != PointerEventData.InputButton.Left) // Ensure the left button was pressed (OR THE FALCON BUTTON)
        {
            return;
        }

        if (objectClicked.tag == "Tag") // A tag was pressed
        {
            Debug.Log(objectClicked.name); // Name of the object
            GameObject currentTag = state.getSelected();
            if (currentTag != null)
            {
                currentTag.GetComponent<Text>().color = Color.black; // Reset the color of the previously selected tag
            }
            state.setSelected(objectClicked);
            objectClicked.GetComponent<Text>().color = Color.red;
        }
        else if (objectClicked.tag == "Bin") // The bin was pressed, so we move the tag to the bin
        {
            Debug.Log("Bin Clicked");
            GameObject currentTag = state.getSelected();
            if (currentTag != null)
            {
                MakeWordBank.replaceTag(currentTag, false);
                currentTag.GetComponent<Text>().color = Color.black; // Reset the color of the previously selected tag
            }
            state.setSelected(null);
        }
        else if (objectClicked.tag == "Image") // The image area was pressed, so here we cast a tag onto the sphere
        {
            Debug.Log("Image Clicked");
            GameObject currentTag = state.getSelected();
			if (currentTag != null && !currentTag.transform.parent.name.Equals("")) // TODO: Check if a tag is currently selected and that the tag isn't blank
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
					GameObject newObject = Instantiate (tagPrefab, hit.point * 0.95f, Quaternion.identity); // Create the new object using the tagPrefab
					newObject.transform.LookAt (Vector3.zero); // Make it face the center of the sphere
					newObject.transform.localScale = new Vector3 (0.2f, 0.1f, 0.00001f);
					newObject.name = "Some Name"; // CHANGE THIS LATER
					newObject.transform.parent = sphere.transform;

					// Create the object which will hold the TextMesh
					GameObject textContainer = new GameObject ();
					textContainer.transform.parent = newObject.transform;
                
					// Create the text mesh to be rendered over the plane
					TextMesh text = textContainer.AddComponent<TextMesh> ();
					text.text = currentTag.transform.parent.name;
					text.fontSize = 20;
					text.alignment = TextAlignment.Center;
					text.anchor = TextAnchor.MiddleCenter;
					text.name = "Some Other Name";
					text.transform.parent = textContainer.transform;
					text.transform.localScale = new Vector3 (-0.1f, 0.25f, 0.25f);
					text.transform.localPosition = Vector3.zero;
					text.transform.localEulerAngles = Vector3.zero;

					MakeWordBank.replaceTag (currentTag, true);
					currentTag.GetComponent<Text> ().color = Color.black;
					state.setSelected (null);




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