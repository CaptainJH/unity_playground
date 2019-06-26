using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpinControlAsset : PlayableAsset
{
    public float m_speed;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SpinControlBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.speed = m_speed;

        return playable;
    }
}
