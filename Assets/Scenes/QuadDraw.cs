using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadDraw : MonoBehaviour
{
    public Texture tex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (tex != null)
        {
            Graphics.Blit(tex, null as RenderTexture);
        }
    }
}
