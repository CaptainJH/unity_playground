using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //LoadAssetsBundle();
        LoadSceneBundle();

        //SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space") && myLoadedAssetBundle)
        {
            myLoadedAssetBundle.Unload(true);
            Debug.Log("Asset bundle unloaded");
        }
	}

    private string bundleFolderName = "AssetBundles";
    private string bundleSceneName = "bundle0";
    private string bundle_assets = "bundle_assets";

    private AssetBundle myLoadedAssetBundle = null;

    void LoadAssetsBundle()
    {
        string path = Path.Combine(Application.dataPath, bundleFolderName);
        path = Path.Combine(path, bundle_assets);
        myLoadedAssetBundle = AssetBundle.LoadFromFile(path);

        if (null == myLoadedAssetBundle)
        {
            Debug.Log("Failed to load AssetBundle: " + path);
            return;
        }
        else
        {
            Debug.Log(path + " load successfully!");
        }
    }

    void LoadSceneBundle()
    {
        sHandle = new SafeFileHandle(hHandle, true);
        if (!sHandle.IsInvalid)
            return;
        sHandle = OpenFileMapping(FILE_MAP_ALL_ACCESS, false, "mmf_bundle0");
        if (sHandle.IsInvalid)
            return;
        int length = 120375;
        pBuffer = MapViewOfFile(sHandle, FILE_MAP_ALL_ACCESS, 0, 0, new UIntPtr((ulong)length));

        byte[] fileContent = new byte[length];
        Marshal.Copy(pBuffer, fileContent, 0, length);


        // (1) load asset bundle
        string path = Path.Combine(Application.dataPath, bundleFolderName);
        path = Path.Combine(path, bundleSceneName);
        //AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(path);
        //byte[] fileContent = File.ReadAllBytes(path);
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromMemory(fileContent);

        if (null == myLoadedAssetBundle)
        {
            Debug.Log("Failed to load AssetBundle: " + path);
            return;
        }
        else
        {
            Debug.Log(path + " load successfully!");
        }

        // (2) extract 'cube' from loaded asset bundle
        //GameObject prefabCube = myLoadedAssetBundle.LoadAsset<GameObject>(resourceName);
        string[] scenePath = myLoadedAssetBundle.GetAllScenePaths();
        if (scenePath.Length > 0)
        {
            SceneManager.LoadScene(scenePath[0], LoadSceneMode.Additive);
        }
    }

    //Shared Memory
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern SafeFileHandle OpenFileMapping(
        uint dwDesiredAccess,
        bool bInheritHandle,
        string lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr MapViewOfFile(
        SafeFileHandle hFileMappingObject,
        UInt32 dwDesiredAccess,
        UInt32 dwFileOffsetHigh,
        UInt32 dwFileOffsetLow,
        UIntPtr dwNumberOfBytesToMap);

    const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
    const UInt32 SECTION_QUERY = 0x0001;
    const UInt32 SECTION_MAP_WRITE = 0x0002;
    const UInt32 SECTION_MAP_READ = 0x0004;
    const UInt32 SECTION_MAP_EXECUTE = 0x0008;
    const UInt32 SECTION_EXTEND_SIZE = 0x0010;
    const UInt32 SECTION_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SECTION_QUERY |
        SECTION_MAP_WRITE |
        SECTION_MAP_READ |
        SECTION_MAP_EXECUTE |
        SECTION_EXTEND_SIZE);
    const UInt32 FILE_MAP_ALL_ACCESS = SECTION_ALL_ACCESS;

    private SafeFileHandle sHandle;
    private IntPtr hHandle;
    private IntPtr pBuffer;
}
