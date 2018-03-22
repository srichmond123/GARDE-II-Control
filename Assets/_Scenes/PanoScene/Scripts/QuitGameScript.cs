using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
	}
	
	// Update is called once per frame
	public static void TaskOnClick () {
		Application.Quit();
		Debug.Log ("QUITTING");
	}
}
