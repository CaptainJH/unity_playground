using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Spin : MonoBehaviour {

    public float speed = 1.0f;

    private string bundleFolderName = "AssetBundles";
    private string bundleName = "bundle_tex";

    // Use this for initialization
    void Start()
    {
        //var tex = LoadAndInstantiateFromAssetBundleLoadFromFile();

        //Renderer renderer = GetComponent<Renderer>();
        //renderer.material.mainTexture = tex;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed, 0, 0);
    }

    Texture2D LoadAndInstantiateFromAssetBundleLoadFromFile()
    {
        // (1) load asset bundle
        string path = Path.Combine(Application.dataPath, bundleFolderName);
        path = Path.Combine(path, bundleName);
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(path);

        if (null == myLoadedAssetBundle)
        {
            Debug.Log("Failed to load AssetBundle: " + path);
            return null;
        }
        else
        {
            Debug.Log(path + " load successfully!");
        }

        // (2) extract 'cube' from loaded asset bundle
        //GameObject prefabCube = myLoadedAssetBundle.LoadAsset<GameObject>(resourceName);
        Texture2D tex = myLoadedAssetBundle.LoadAsset<Texture2D>("Floor4");
        if(null == tex)
        {
            Debug.Log("texture load failed!");
        }
        else
        {
            Debug.Log("texture load successfully !");
        }

        return tex;
    }
}
