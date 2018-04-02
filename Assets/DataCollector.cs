using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DataCollector : MonoBehaviour
{

    static int sequenceIndex = 0;
    static int imageIndex = 0;
    static int numTagsRemaining = 5;
    static int userID = 0;

    static float elapsedTime; // Running time of the app

    static List<float> times; // Time of each logged frame
    static List<Vector3> cameraRot; // The angle of the camera at the logged frame
    static List<Vector3> falconPos; // The position of the Falcon joystick at the logged frame

	static List<TagInfo> tagInfo;

    static Vector3 tempPos;

    static GameObject cam;

    static string dataPath = "Data/";
	static string userPath = "";

    static bool falcon; // Is the controller on?
	static bool writtenPanDataColumnNames = false;

    // Use this for initialization
    void Start()
    {
        falcon = GameObject.Find("Falcon") != null;
        cam = GameObject.Find("Main Camera");
        cameraRot = new List<Vector3>();
        times = new List<float>();
        falconPos = new List<Vector3>();
        elapsedTime = 0f;
        Directory.CreateDirectory(dataPath);
        userID = Directory.GetDirectories(dataPath).Length;

		tagInfo = new List<TagInfo> ();

		userPath = dataPath + "User-" + userID + '/';
		Directory.CreateDirectory(userPath);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        //times.Add(elapsedTime);
        //cameraRot.Add(cam.transform.localEulerAngles);

		if (falcon)
		{
			FalconUnity.getTipPosition(0, out tempPos);
			falconPos.Add(tempPos);
		}

		string path = userPath + "panData.csv";

		new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write).Close();
		StreamWriter streamWriter = new StreamWriter(path, true, Encoding.ASCII);
		if (!writtenPanDataColumnNames) { //Only write this once:
			streamWriter.Write ("time (s), rotation.x, rotation.y, falcon.x, falcon.y, falcon.z\n");
			writtenPanDataColumnNames = true;
		}
		string line = elapsedTime.ToString () + "," +
			cam.transform.localEulerAngles.x.ToString () + "," + cam.transform.localEulerAngles.y.ToString ();
		if (falcon) {
			line += "," + tempPos.x.ToString () + "," + tempPos.y.ToString () + tempPos.z.ToString() + "\n";
		} else {
			line += "\n";
		}
		streamWriter.Write(line);
		streamWriter.Close();
		/* Not needed if it's writing every frame:
		times.Clear();
		cameraRot.Clear();
		falconPos.Clear();
		*/
    }

    public static void Flush() // Write current data to csv file
    {
		/*
        if (!falcon)
        {
            return;
        }

        //Debug.Log("FLUSHING");
		//I'm doing this in the AddTag method so it writes to memory for each tag added:
        Transform tagTransform = GameObject.Find("TagSphere").transform; // Tag Container

        string userPath = dataPath + "User-" + userID + '/';
        Directory.CreateDirectory(userPath);

        // Log tag data
        //string imgName = tagTransform.gameObject.GetComponent<Renderer>().material.name; // Name of the image file
        //string path = userPath + imgName + ".csv";
        //new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write).Close(); // Create the file
        //StreamWriter streamWriter = new StreamWriter(path, true, Encoding.ASCII);         // Open the file
		//streamWriter.Write("posx, posy, posz, tag, time (s), userID\n");   // Write header line to the file

		
        foreach (Transform t in tagTransform)
        {
            if (t != tagTransform)
            {
                Vector3 position = t.gameObject.GetComponent<Renderer>().bounds.center;
                string line = position.x.ToString() + "," +
                              position.y.ToString() + "," + 
                              position.z.ToString() + "," +
                              t.gameObject.name + "," + 
                              userID.ToString() + "\n";
                streamWriter.Write(line);
            }
        }
		foreach (TagInfo tg in tagInfo) {
			string line = tg.position.x.ToString () + "," +
			              tg.position.y.ToString () + "," +
			              tg.position.z.ToString () + "," +
			              tg.name + "," +
						  tg.timeStamp.ToString() + "," + 
			              tg.userID.ToString () + "\n";
			streamWriter.Write (line);
		}
        streamWriter.Close(); // Close the file after writing

		

        // Log all of falcon/camera data - should this only happen during image turnover?
        string path = userPath + "panData.csv";

        new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write).Close();
        StreamWriter streamWriter = new StreamWriter(path, true, Encoding.ASCII);
		streamWriter.Write("time (s), rotation.x, rotation.y, falcon.x, falcon.y\n");
        for (int i = 0; i < times.Count; i++)
        {
			string line = times [i].ToString () + "," +
			                       cameraRot [i].x.ToString () + "," + cameraRot [i].y.ToString ();
			if (falcon) {
				line += "," + falconPos [i].x.ToString () + "," + falconPos [i].y.ToString () + "\n";
			} else {
				line += "\n";
			}
            streamWriter.Write(line);
        }
        streamWriter.Close();
        times.Clear();
        cameraRot.Clear();
        falconPos.Clear();
		*/

		//Only part that should remain from .Flush() method called on image turnover:
		tagInfo.Clear ();
    }

	public static void AddTag(string name, Vector3 position = new Vector3()) {
		TagInfo nTagInfo;
		nTagInfo.position = position;
		nTagInfo.name = name;
		nTagInfo.userID = userID;
		nTagInfo.timeStamp = elapsedTime;
		tagInfo.Add (nTagInfo);

		//Write to memory here instead of Flush() :

		Transform tagTransform = GameObject.Find("TagSphere").transform;
		string userPath = dataPath + "User-" + userID + '/';
		Directory.CreateDirectory(userPath);

		// Log tag data
		string imgName = tagTransform.gameObject.GetComponent<Renderer>().material.name; // Name of the image file
		string path = userPath + imgName + ".csv";
		new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write).Close(); // Create the file, set to FileMode.Create so it overwrites each time
		StreamWriter streamWriter = new StreamWriter(path, true, Encoding.ASCII);         // Open the file
		streamWriter.Write("posx, posy, posz, tag, time (s), userID\n");

		foreach (TagInfo tg in tagInfo) {
			string line = tg.position.x.ToString () + "," +
				tg.position.y.ToString () + "," +
				tg.position.z.ToString () + "," +
				tg.name + "," +
				tg.timeStamp.ToString() + "," + 
				tg.userID.ToString () + "\n";
			streamWriter.Write (line);
		}
		streamWriter.Close(); // Close the file after writing
	}
}

public struct TagInfo {
	public Vector3 position;
	public string name;
	public int userID;
	public float timeStamp;
};
