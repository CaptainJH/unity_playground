using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovicClipMaster : MovieClip
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovieClip();
    }

    public void On100()
    {
        Stop();
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var childObj = transform.GetChild(0).gameObject;
            childObj.SetActive(true);
            var mc = childObj.GetComponentInChildren<MovieClipSlave>();
            mc.Play();
        }
    }
}
