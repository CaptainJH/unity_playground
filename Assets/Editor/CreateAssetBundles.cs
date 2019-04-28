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
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach(object o in obj)
        {
            GameObject g = (GameObject)o;
            Debug.Log(g.name);
        }
    }

    [MenuItem("Assets/Assets Dependencies")]
    static void GetAssetsDependencies()
    {
        var obj = Selection.activeObject;
        Object[] roots = new Object[] { obj };

        var dep = EditorUtility.CollectDependencies(roots);
        foreach(var o in dep)
        {
            if (AssetDatabase.Contains(o))
            {
                string assetPath = AssetDatabase.GetAssetPath(o);
                Debug.Log(assetPath);
            }
        }
        //Selection.objects = dep;
    }
}