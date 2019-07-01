using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SpinControlMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        GameObject objectBinding = playerData as GameObject;
        if (!objectBinding)
            return;

        Vector3 finalPosition = Vector3.zero;
        Vector3 finalScale = Vector3.one;

        int inputCount = playable.GetInputCount();
        for(int i = 0; i < inputCount; ++i)
        {
            float inputWeight = playable.GetInputWeight(i);
            var inputPlayable = (ScriptPlayable<SpinControlBehaviour>)playable.GetInput(i);
            var input = inputPlayable.GetBehaviour();

            finalPosition += input.position * inputWeight;
            finalScale += input.Scale * inputWeight;
        }

        var transform = objectBinding.GetComponent<Transform>();
        if(transform)
        {
            objectBinding.transform.position = finalPosition;
            objectBinding.transform.localScale = finalScale;
        }
    }
}
