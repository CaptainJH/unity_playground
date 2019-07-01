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
    private bool isReversePlay = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    protected void Init()
    {
        timelineDirector.timeUpdateMode = DirectorUpdateMode.Manual;
        foreach(var track in timelineDirector.playableAsset.outputs)
        {
            if(track.sourceObject)
            {
                var obj = timelineDirector.GetGenericBinding(track.sourceObject);
                if(obj)
                {
                    var mc = (obj as GameObject).GetComponentInChildren<MovieClip>();
                }
            }
        }

        var methodInfos = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach(var method in methodInfos)
        {
            if(method.Name.StartsWith("On"))
            {
                methods.Add(method);
            }
        }
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
    }

    public void UpdateMovieClip()
    {
        if (!isPaused)
        {
            if (isReversePlay)
            {
                timelineDirector.playableGraph.Evaluate(-Time.deltaTime);
            }
            else
            {
                timelineDirector.playableGraph.Evaluate(Time.deltaTime);
            }
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
        isReversePlay = false;
    }

    public void PlayRevese()
    {
        isPaused = false;
        isReversePlay = true;
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

    public List<int> GetAllFramesWithScript()
    {
        List<int> ret = new List<int>();
        var methodInfos = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach (var method in methodInfos)
        {
            if (method.Name.StartsWith("On"))
            {
                int f = int.Parse(method.Name.Replace("On", ""));
                if(f >= 0)
                {
                    ret.Add(f);
                }
            }
        }
        return ret;
    }
}
