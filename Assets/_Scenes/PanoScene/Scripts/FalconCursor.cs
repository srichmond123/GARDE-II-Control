// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class FalconCursor : MonoBehaviour
{
	private GameObject falcon;
	private GameObject cursor;

	private Vector3 falconPos;
	private Vector3 offset;

	private bool[] buttons;

	public float sensitivity = 0.7f;

	private void Start()
	{
		this.falcon = GameObject.Find("Tip");
		this.cursor = GameObject.Find("CursorSphere");
	}

	private void Update()
	{
		if (!this.falcon)
		{
			return;
		}
		this.falconPos = this.falcon.transform.localPosition;
		FalconUnity.getFalconButtonStates(0, out this.buttons);
		this.cursor.transform.localPosition = new Vector3(this.falconPos.x * 0.21333f * this.sensitivity, this.falconPos.y * 0.12f * this.sensitivity, 0.418f);
	}
}
