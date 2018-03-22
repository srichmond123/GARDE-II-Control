using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour {

    private int sequenceIndex = 0;
    private int imageIndex = 0; 
    private int numTagsRemaining = 5;

    private float elapsedTime;

    private List<float> times;
    private List<Vector3> cameraRot;
    private List<Vector3> falconPos;

    private Vector3 tempPos;

    private GameObject cam;

    private string dataPath = "Data/";

    // Use this for initialization
    void Start () {
        this.cam = GameObject.Find("Main Camera");
        this.cameraRot = new List<Vector3>();
        this.times = new List<float>();
        this.falconPos = new List<Vector3>();
        this.elapsedTime = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        this.elapsedTime += Time.deltaTime;
        this.times.Add(this.elapsedTime);
        this.cameraRot.Add(this.cam.transform.localEulerAngles);
        FalconUnity.getTipPosition(0, out this.tempPos);
        this.falconPos.Add(this.tempPos);
    }

    void Flush () // Write current data to cav file
    {

    }
}
