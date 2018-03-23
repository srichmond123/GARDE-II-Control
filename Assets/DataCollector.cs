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

    static float elapsedTime;

    static List<float> times;
    static List<Vector3> cameraRot;
    static List<Vector3> falconPos;

    static Vector3 tempPos;

    static GameObject cam;

    static string dataPath = "Data/";

    static bool falcon; // Is the controller on?

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!falcon) // There's no falcon, so no data collection
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        times.Add(elapsedTime);
        cameraRot.Add(cam.transform.localEulerAngles);
        FalconUnity.getTipPosition(0, out tempPos);
        falconPos.Add(tempPos);
    }

    public static void Flush() // Write current data to csv file
    {
        if (!falcon)
        {
            return;
        }
        Debug.Log("FLUSHING");
        Transform tagTransform = GameObject.Find("TagSphere").transform; // Tag Container
        string userPath = dataPath + "User-" + userID + '/';
        Directory.CreateDirectory(userPath);

        // Log tag data
        string imgName = tagTransform.gameObject.GetComponent<Renderer>().material.name;
        string path = userPath + imgName + ".csv";
        new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write).Close();
        StreamWriter streamWriter = new StreamWriter(path, true, Encoding.ASCII);
        streamWriter.Write("posx, posy, posz, tag, userID\n");
        foreach (Transform t in tagTransform)
        {
            if (t != tagTransform)
            {
                Vector3 position = t.gameObject.GetComponent<Renderer>().bounds.center;
                string line = position.x.ToString() + "," +
                              position.y.ToString() + "," + 
                              position.x.ToString() + "," +
                              t.gameObject.name + "," + 
                              userID.ToString() + "\n";
                streamWriter.Write(line);
            }
        }
        streamWriter.Close();

        // Log all of falcon/camera data
        path = userPath + "panData.csv";

        new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write).Close();
        streamWriter = new StreamWriter(path, true, Encoding.ASCII);
        streamWriter.Write("time, rotation.x, rotation.y, falcon.x, falcon.y\n");
        for (int i = 0; i < times.Count; i++)
        {
            string line = times[i].ToString() + "," +
                          cameraRot[i].x.ToString() + "," + cameraRot[i].y.ToString() + "," +
                          falconPos[i].x.ToString() + "," + falconPos[i].y.ToString() + "\n";
            streamWriter.Write(line);
        }
        streamWriter.Close();
        times.Clear();
        cameraRot.Clear();
        falconPos.Clear();
    }
}
