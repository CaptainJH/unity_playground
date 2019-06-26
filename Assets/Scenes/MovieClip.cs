using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MovieClip : MonoBehaviour
{
    public PlayableDirector timelineDirector;

    // Start is called before the first frame update
    void Start()
    {
        timelineDirector.timeUpdateMode = DirectorUpdateMode.Manual;
    }

    // Update is called once per frame
    void Update()
    {
        timelineDirector.playableGraph.Evaluate(Time.deltaTime);
    }

    public virtual void Goto(int frame)
    {

    }

    public virtual void GotoAndPlay(int frame)
    {

    }
}
