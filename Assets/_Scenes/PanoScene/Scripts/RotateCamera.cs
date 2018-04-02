using UnityEngine;

public class RotateCamera : MonoBehaviour
{
	private Vector3 mousepos;
	private Vector3 falconpos;
	private Vector3 nextpos;

	public float sensitivity = 5f;
	private float zoom;

	private bool[] buttons;
	private bool panning;

	private GameObject cam;
	private GameObject falcon;
    StateManager state; // State of the application

    private Rect cameraBounds;

	private void Start() {
		this.mousepos = Input.mousePosition; // Tracking the mouse position in case the controller isn't connected
		this.zoom = 0f;
        this.buttons = new bool[] { false, false, false, false };

        this.cam = GameObject.Find("Main Camera");
        // Get camera bounds
        cameraBounds = cam.GetComponent<Camera>().pixelRect;

		if (this.falcon = GameObject.Find("Tip"))
        {
            this.falconpos = Vector3.zero;
        }

        state = GameObject.Find("Canvas").GetComponent<StateManager>();

        this.panning = false;
	}

	private void Update()
	{
        
        if (!this.falcon) // Not using the contoller, so mouse is fine
		{
			if (Input.mouseScrollDelta.y != 0f)
			{
				this.zoom += Mathf.Sign(Input.mouseScrollDelta.y) * 0.1f;
			}
            // Enforce a boundary on the zoom
            this.zoom = Mathf.Max(Mathf.Min(0.8f, this.zoom), 0f);

            // The new camera rotation will depend on the movement of the mouse
			this.nextpos = new Vector3(base.transform.localEulerAngles.x - Input.mousePosition.y + this.mousepos.y, base.transform.localEulerAngles.y + Input.mousePosition.x - this.mousepos.x, 0f);
			this.mousepos = Input.mousePosition;
		}
		else // Using the controller
		{
			FalconUnity.getFalconButtonStates(0, out this.buttons); // Which buttons are currently pressed?
            //Set the new position based on the movement of the controller
            Vector3 cursorPosition = state.getCursorPosition();

            float newX = transform.localEulerAngles.x - (cursorPosition.y - cameraBounds.center.y) / 1000 * this.sensitivity;
            float newY = transform.localEulerAngles.y + (cursorPosition.x - cameraBounds.center.x) / 1000 * this.sensitivity;
            this.nextpos = new Vector3(newX, newY, 0f);
		}

        // Update the zoom
		this.cam.transform.localPosition = new Vector3(this.cam.transform.localPosition.x, this.cam.transform.localPosition.y, this.zoom);

        // Enforce a boundary on rotating up/down
		if (this.nextpos.x > 270f && this.nextpos.x < 280f)
		{
			this.nextpos.x = 280f;
		}
		else if (this.nextpos.x > 35f && this.nextpos.x < 90f)
		{
			this.nextpos.x = 35f;
		}

        // If the pan button is pressed (right click on mouse, or left button on controller), update the camera rotations
		if ((this.falcon && this.buttons[0]) || Input.GetMouseButton(1))
		{
			base.transform.localEulerAngles = this.nextpos;
			this.panning = true;
		}
		else
		{
			this.panning = false;
		}
	}

	public bool isPanning() {
		return this.panning;
	}
}
