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
        if (GetMarkerCount() == 0)
        {
            var director = go.GetComponentInChildren<PlayableDirector>();
            var obj = director.GetGenericBinding(this);
            var mc = (obj as GameObject).GetComponentInChildren<MovieClip>();
            var frameList = mc.GetAllFramesWithScript();
            foreach(var f in frameList)
            {
                var time = f / 60.0f;
                CreateMarker<TestMarker>(time);
            }
        }
        return ScriptPlayable<SpinControlMixerBehaviour>.Create(graph, inputCount);
    }
}
