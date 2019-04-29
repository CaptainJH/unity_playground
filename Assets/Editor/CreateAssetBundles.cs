using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles/Mac")]
    static void BuildAllAssetBundlesMac()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
    }

    [MenuItem("Assets/Build AssetBundles/Windows")]
    static void BuildAllAssetBundlesWindows()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Assets/Scene Objects Iteration")]
    static void IterateSceneObjects()
    {
        var obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach(var o in obj)
        {
            GameObject g = (GameObject)o;
            Debug.Log(g.name);
        }

        var depAssets = GetAssetsDependencies(obj);
        Selection.objects = depAssets.ToArray();
    }

    //[MenuItem("Assets/Assets Dependencies")]
    //static void GetSelectedAssetDependencies()
    //{
    //    var obj = Selection.activeObject;
    //    Object[] roots = new Object[] { obj };

    //    var dep = GetAssetsDependencies(roots);
    //    foreach(var o in dep)
    //    {
    //        if (AssetDatabase.Contains(o))
    //        {
    //            string assetPath = AssetDatabase.GetAssetPath(o);
    //            Debug.Log(assetPath);
    //        }
    //    }
    //    //Selection.objects = dep;
    //}

    static List<Object> GetAssetsDependencies(Object[] objsIn)
    {
        List<Object> ret = new List<Object>();
        var deps = EditorUtility.CollectDependencies(objsIn);

        foreach(var obj in deps)
        {
            if(AssetDatabase.Contains(obj))
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                if (assetPath.StartsWith("Assets/Scenes/"))
                {
                    if (obj is Material || obj is Texture2D || obj is Mesh)
                    {
                        ret.Add(obj);
                    }
                }
            }
        }
        return ret;
    }
}