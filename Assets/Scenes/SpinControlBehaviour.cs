using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;

public class SpinControlBehaviour : PlayableBehaviour
{
    public Vector3 position;
    public Vector3 Scale;

    //public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    //{   
    //    var obj = playerData as GameObject;
    //    var mc = obj.GetComponent<MovieClip>();
    //    var transform = obj.GetComponent<Transform>();
    //    if(transform)
    //    {
    //        obj.transform.position = position;
    //    }

    //    //if (obj != null)
    //    //{
    //    //    obj.transform.Rotate(speed, 0, 0);
    //    //}
    //}
}
