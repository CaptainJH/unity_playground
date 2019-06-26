using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ForwardPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var meshFilter = GetComponentInParent<MeshFilter>();
        var mesh = meshFilter.mesh;
        unsafe
        {
            System.IntPtr ib = mesh.GetNativeIndexBufferPtr();
            if (ib.ToPointer() == null)
            {
                return;
            }
            System.IntPtr vb = mesh.GetNativeVertexBufferPtr(0);
            if (vb.ToPointer() == null)
            {
                return;
            }
        }

        var ptr = GetRenderEventFunc();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // SetupResources and GetRenderEventFunc are the two methods found in our native rendering plugin.
    [DllImport("UnityPlugin")]
    private static extern void SetupResources(System.IntPtr nativeIndexBuffer, System.IntPtr nativeVertexBuffer, System.IntPtr nativeTextureResource);

    // GetRenderEventFunc will return a native function pointer to our native DoRenderEvent method
    // which will ultimately intialize our native graphics state object and handle the native rendering of
    // our mesh.
    [DllImport("UnityPlugin")]
    private static extern System.IntPtr GetRenderEventFunc();
}
