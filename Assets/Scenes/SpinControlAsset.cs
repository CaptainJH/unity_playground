using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SpinControlAsset : PlayableAsset, ITimelineClipAsset
{
    public Vector3 position;
    public Vector3 scale;
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
        behaviour.Scale = scale;
        

        return playable;
    }
}
