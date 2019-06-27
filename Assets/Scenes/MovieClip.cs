using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;

public class MovieClip : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    private List<MethodInfo> methods = new List<MethodInfo>();
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        timelineDirector.timeUpdateMode = DirectorUpdateMode.Manual;

        var methodInfos = typeof(MovieClip).GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach(var method in methodInfos)
        {
            if(method.Name.StartsWith("On"))
            {
                methods.Add(method);
            }
        }
    }

    public void On100()
    {
        Stop();
    }

    public int CurrentFrame
    {
        get
        {
            var currentTime = timelineDirector.time;
            return (int)(currentTime * 60 + 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            timelineDirector.playableGraph.Evaluate(Time.deltaTime);
        }
        var t = timelineDirector.time;

        int frame = CurrentFrame;
        string frameFuntion = "On" + frame.ToString();
        foreach(var method in methods)
        {
            if(method.Name == frameFuntion)
            {
                method.Invoke(this, null);
            }
        }
    }

    public virtual void Play()
    {
        isPaused = false;
    }

    public virtual void Stop()
    {
        isPaused = true;
    }

    public virtual void Goto(int frame)
    {

    }

    public virtual void GotoAndPlay(int frame)
    {

    }
}
