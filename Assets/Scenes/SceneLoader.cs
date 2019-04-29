using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // (1) load asset bundle
        string path = Path.Combine(Application.dataPath, bundleFolderName);
        path = Path.Combine(path, bundleSceneName);
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(path);

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
}
