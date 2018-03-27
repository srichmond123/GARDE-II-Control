using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FalconPointer : PointerInputModule
{
    private GameObject falcon;
    private ClickAction eventListener;

    private Vector3 falconpos;

    private bool[] buttons;
    private bool prevClick; // To know if the button had been pressed in the last frame

    public override void Process()
	{
		PointerEventData falconEventData = this.GetFalconEventData();
	}

    public void Start()
    {
        this.buttons = new bool[] { false, false, false, false }; // Buttons on the Falcon
        if (this.falcon = GameObject.Find("Tip"))
        {
            this.falconpos = Vector3.zero;
            eventListener = GameObject.Find("Canvas").GetComponent<ClickAction>();
        }
    }

    public void Update()
    {
        if (falcon)
        {
            FalconUnity.getFalconButtonStates(0, out this.buttons); // Which buttons are currently pressed?
            if (!prevClick && (this.buttons[3] || buttons[1]))
            {
                Process();
                eventListener.OnPointerClick(GetFalconEventData());
            }
            prevClick = (this.buttons[3] || buttons[1]);
        }
    }

	protected virtual PointerEventData GetFalconEventData()
	{
		PointerEventData pointerEventData;
		bool pointerData = base.GetPointerData(-1, out pointerEventData, true);
		pointerEventData.Reset();
		if (pointerData)
		{
			pointerEventData.position = GameObject.Find("CursorCamera").GetComponent<Camera>().WorldToScreenPoint(base.gameObject.transform.position);
		}
		Vector2 vector = GameObject.Find("CursorCamera").GetComponent<Camera>().WorldToScreenPoint(base.gameObject.transform.position);
		pointerEventData.delta = vector - pointerEventData.position;
		pointerEventData.position = vector;
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(vector), out raycastHit))
		{
			pointerEventData.pointerCurrentRaycast = new RaycastResult
			{
				gameObject = raycastHit.transform.gameObject
			};
		}
		ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(pointerEventData.pointerCurrentRaycast.gameObject, pointerEventData, ExecuteEvents.pointerDownHandler);
		base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
		RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
		pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
        pointerEventData.button = PointerEventData.InputButton.Left;
		this.m_RaycastResultCache.Clear();
		return pointerEventData;
	}
}
