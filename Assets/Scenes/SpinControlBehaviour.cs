using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpinControlBehaviour : PlayableBehaviour
{
    public float speed;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {   
        var obj = playerData as GameObject;
        var mc = obj.GetComponent<MovieClip>();
        //if (obj != null)
        //{
        //    obj.transform.Rotate(speed, 0, 0);
        //}
    }
}
