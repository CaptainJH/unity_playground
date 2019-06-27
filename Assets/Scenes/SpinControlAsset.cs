using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SpinControlAsset : PlayableAsset, ITimelineClipAsset
{
    //public float m_speed;
    public Vector3 position;
    public Dictionary<string, object> valueTable = new Dictionary<string, object>();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SpinControlBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.position = position;
        //behaviour.speed = m_speed;

        return playable;
    }
}
