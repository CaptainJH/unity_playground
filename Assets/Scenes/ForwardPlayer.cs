using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class ForwardPlayer : MonoBehaviour
{
    private static string ForwardPath = "C:/Users/heqi/Documents/forward/";
    public Camera m_Camera = null;

    enum CustomRenderEvent
    {
        // 3245 is a random number I made up.
        // I figured it could be useful to send an event id
        // to the native plugin which corresponds to when
        // the mesh is being rendered in the render pipeline.
        AfterForwardOpaque = 3245,
    }

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

        SetupForwardPath(ForwardPath);
        var rt = m_Camera.targetTexture;
        if (rt != null)
        {
            var nativeRTPtr = rt.GetNativeTexturePtr();
            SetupResources("DefaultRT", nativeRTPtr);
        }

        var ptr = GetRenderEventFunc();

        cb = new CommandBuffer();
    }

    // Update is called once per frame
    void Update()
    {
        OnPreRender();
    }

    CommandBuffer cb = null;

    private void OnPreRender()
    {
        // If we didn't care about updating the matrix data dynamically,
        // we could just attach a single command buffer to our camera in the Start method.
        // However because we want our model to rotate, we need to update the matrix data in
        // the native rendering plugin.
        m_Camera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, cb);
        cb.Release();

        // Don't pass the camera's projection matrix directly into the plugin.
        // We need to calculate the GPU ready version of the projection matrix
        // which enable the projection matrix to play nicely with reverse z depth
        // testing and inverted uv coordinates to go from opengl to d3d11.
        // The projection matrix will also be modified such that the normalized device
        // coordinates for the z component will be converted from -1 to 1 (OpenGL) to 0 to 1 (DX11).
        Matrix4x4 projectionMatrix = m_Camera.projectionMatrix;
        projectionMatrix = GL.GetGPUProjectionMatrix(projectionMatrix, true);

        //MatrixToFloatArray(m_WorldObjectTransform.localToWorldMatrix, ref nativeRenderingData.localToWorldMatrix);
        //MatrixToFloatArray(m_Camera.worldToCameraMatrix, ref nativeRenderingData.worldToViewMatrix);
        //MatrixToFloatArray(projectionMatrix, ref nativeRenderingData.viewToProjectionMatrix);

        // Copy our managed nativeRenderingData into the unmanaged memory pointed to by
        // the nativeRenderingDataPtr pointer.
        //Marshal.StructureToPtr(nativeRenderingData, nativeRenderingDataPtr, true);

        cb = new CommandBuffer();
        cb.name = "Native Rendering Plugin";
        // The IssuePluginEventAndData will ensure our native rendering function is executed and receives
        // our custom rendering data when the command buffer is executed on the render thread.
        //cb.IssuePluginEventAndData(GetRenderEventFunc(), (int)CustomRenderEvent.AfterForwardOpaque, nativeRenderingDataPtr);
        cb.IssuePluginEvent(GetRenderEventFunc(), (int)CustomRenderEvent.AfterForwardOpaque);
        m_Camera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, cb);
    }

    // There might be a better way to do this but
    // I want to marshal the C# Matrix4x4 data to C++
    // without any marshaling issues.
    void MatrixToFloatArray(Matrix4x4 m, ref float[] outputFloatArray)
    {
        outputFloatArray[0] = m.m00;
        outputFloatArray[1] = m.m01;
        outputFloatArray[2] = m.m02;
        outputFloatArray[3] = m.m03;

        outputFloatArray[4] = m.m10;
        outputFloatArray[5] = m.m11;
        outputFloatArray[6] = m.m12;
        outputFloatArray[7] = m.m13;

        outputFloatArray[8] = m.m20;
        outputFloatArray[9] = m.m21;
        outputFloatArray[10] = m.m22;
        outputFloatArray[11] = m.m23;

        outputFloatArray[12] = m.m30;
        outputFloatArray[13] = m.m31;
        outputFloatArray[14] = m.m32;
        outputFloatArray[15] = m.m33;
    }

    // SetupResources and GetRenderEventFunc are the two methods found in our native rendering plugin.
    [DllImport("UnityPlugin")]
    private static extern void SetupResources(string name, System.IntPtr nativeResourcePtr);

    // GetRenderEventFunc will return a native function pointer to our native DoRenderEvent method
    // which will ultimately intialize our native graphics state object and handle the native rendering of
    // our mesh.
    [DllImport("UnityPlugin")]
    private static extern System.IntPtr GetRenderEventFunc();

    [DllImport("UnityPlugin")]
    private static extern void SetupForwardPath(string path);
}
