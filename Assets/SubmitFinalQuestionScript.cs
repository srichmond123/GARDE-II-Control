using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitFinalQuestionScript : MonoBehaviour {

	// Use this for initialization
	public Slider slide;
    public Slider slide2;
	public static bool startListening = false;
    public static bool isListening = false;
	bool notAwake = true;

	void Start() {
	}
	
	// Update is called once per frame
	void Update () {
		if (startListening) {
            isListening = true;
			Button btn = gameObject.GetComponent<Button> ();
			btn.onClick.AddListener (TaskOnClick);
			startListening = false;
		}
	}

	void TaskOnClick() {
		DataCollector.writeFinalQuestion ((int) slide.value, (int) slide2.value);
		Debug.Log ("Clicked Submit button");
		Application.Quit ();
	}
}
