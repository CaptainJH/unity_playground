using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

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

public class AssetBundleWindow : EditorWindow
{
    public static AssetBundleWindow instance;

    public static void ShowAssetBundleWindow()
    {
        instance = (AssetBundleWindow)EditorWindow.GetWindow(typeof(AssetBundleWindow));
        instance.titleContent = new GUIContent("Asset Bundle Window");
    }

    private void OnGUI()
    {
        //EditorGUILayout.LabelField("The GUI of this window is modified.");
        DrawTabs();
    }

    private int selectedTabIndex = 0;

    private void DrawTabs()
    {
        selectedTabIndex = GUILayout.Toolbar(selectedTabIndex, new string[] { "Material", "Texture", "Mesh", "Others" });
    }

    [MenuItem("Window/Asset Bundle Window")]
    private static void ShowWindow()
    {
        AssetBundleWindow.ShowAssetBundleWindow();
    }
}

public class SimpleTreeViewWindow : EditorWindow
{
    UnityEditor.IMGUI.Controls.TreeViewState m_TreeViewState;
    MultiColumnHeaderState m_MultiColumnHeaderState;

    SimpleTreeView m_TreeView;

    void OnEnable()
    {
        if (m_TreeViewState == null)
            m_TreeViewState = new UnityEditor.IMGUI.Controls.TreeViewState();

        m_MultiColumnHeaderState = CreateDefaultMultiColumnHeaderState(multiColumnTreeViewRect.width);
        var multiColumnHeader = new MyMultiColumnHeader(m_MultiColumnHeaderState);
        multiColumnHeader.ResizeToFit();

        m_TreeView = new SimpleTreeView(m_TreeViewState, multiColumnHeader);
    }

    void OnGUI()
    {
        DoTreeView();
    }

    Rect multiColumnTreeViewRect
    {
        get { return new Rect(20, 30, position.width - 40, position.height - 60); }
    }

    void DoTreeView()
    {
        m_TreeView.OnGUI(multiColumnTreeViewRect);
    }

    [MenuItem("Window/Tree View Window")]
    static void ShowWindow()
    {
        var window = GetWindow<SimpleTreeViewWindow>();
        window.titleContent = new GUIContent("Treeview window");
        window.Show();
    }


    public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
    {
        var columns = new[]
        {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByLabel")),
                    contextMenuText = "Asset",
                    headerTextAlignment = TextAlignment.Center,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Right,
                    width = 30,
                    minWidth = 30,
                    maxWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByType")),
                    contextMenuText = "Type",
                    headerTextAlignment = TextAlignment.Center,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Right,
                    width = 30,
                    minWidth = 30,
                    maxWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Name"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 150,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Multiplier"),
                    headerTextAlignment = TextAlignment.Right,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Left,
                    width = 110,
                    minWidth = 60,
                    autoResize = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Material"),
                    headerTextAlignment = TextAlignment.Right,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Left,
                    width = 95,
                    minWidth = 60,
                    autoResize = true,
                    allowToggleVisibility = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Note"),
                    headerTextAlignment = TextAlignment.Right,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Left,
                    width = 70,
                    minWidth = 60,
                    autoResize = true
                }
            };

        //Assert.AreEqual(columns.Length, Enum.GetValues(typeof(MyColumns)).Length, "Number of columns should match number of enum values: You probably forgot to update one of them.");

        var state = new MultiColumnHeaderState(columns);
        return state;
    }
}


internal class MyMultiColumnHeader : MultiColumnHeader
{
    Mode m_Mode;

    public enum Mode
    {
        LargeHeader,
        DefaultHeader,
        MinimumHeaderWithoutSorting
    }

    public MyMultiColumnHeader(MultiColumnHeaderState state)
        : base(state)
    {
        mode = Mode.DefaultHeader;
    }

    public Mode mode
    {
        get
        {
            return m_Mode;
        }
        set
        {
            m_Mode = value;
            switch (m_Mode)
            {
                case Mode.LargeHeader:
                    canSort = true;
                    height = 37f;
                    break;
                case Mode.DefaultHeader:
                    canSort = true;
                    height = DefaultGUI.defaultHeight;
                    break;
                case Mode.MinimumHeaderWithoutSorting:
                    canSort = false;
                    height = DefaultGUI.minimumHeight;
                    break;
            }
        }
    }

    protected override void ColumnHeaderGUI(MultiColumnHeaderState.Column column, Rect headerRect, int columnIndex)
    {
        // Default column header gui
        base.ColumnHeaderGUI(column, headerRect, columnIndex);

        // Add additional info for large header
        if (mode == Mode.LargeHeader)
        {
            // Show example overlay stuff on some of the columns
            if (columnIndex > 2)
            {
                headerRect.xMax -= 3f;
                var oldAlignment = EditorStyles.largeLabel.alignment;
                EditorStyles.largeLabel.alignment = TextAnchor.UpperRight;
                GUI.Label(headerRect, 36 + columnIndex + "%", EditorStyles.largeLabel);
                EditorStyles.largeLabel.alignment = oldAlignment;
            }
        }
    }
}
