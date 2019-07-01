using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieClipSlave : MovieClip
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
        Stop();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void On70()
    {
        Stop();
    }

    private void OnMouseOver()
    {
        Play();
    }

    private void OnMouseExit()
    {
        PlayRevese();
    }
}
