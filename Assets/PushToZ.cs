using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PushToZ : MonoBehaviour {

    Vector3 pos;
    public int k = 50;

    void push()
    {
        FalconUnity.getTipPosition(0, out pos);
        //if (Mathf.Abs(pos.z - wall) > 0.15f)
        //    FalconUnity.setForceField(0, 500 * Vector3.back);
        //FalconUnity.applyForce(0, 50*(pos.z-wall) * Vector3.back, 0.0001f);
        //    FalconUnity.setForceField(0, 1000*(pos.z-wall) * Vector3.back);
        //else
        //    FalconUnity.setForceField(0, Vector3.zero);
        FalconUnity.applyForce(0, k * pos.z * Vector3.back, 0.001f);
    }

    // Use this for initialization
    void Start () {
        AutoResetEvent a = new AutoResetEvent(false);
        Timer timer = new Timer((e) => { push(); }, a, 1000, 1);
    }
}
