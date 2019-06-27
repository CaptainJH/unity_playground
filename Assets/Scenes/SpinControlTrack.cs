using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(SpinControlAsset))]
[TrackBindingType(typeof(GameObject))]
public class SpinControlTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SpinControlMixerBehaviour>.Create(graph, inputCount);
    }
}
