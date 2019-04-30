using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using Random = UnityEngine.Random;


public class TreeElement
{
    int m_ID;
    string m_Name;
    int m_Depth;

    public int depth
    {
        get { return m_Depth; }
        set { m_Depth = value; }
    }

    public string name
    {
        get { return m_Name; }
        set { m_Name = value; }
    }

    public int id
    {
        get { return m_ID; }
        set { m_ID = value; }
    }

    public TreeElement()
    {
    }

    public TreeElement(string name, int depth, int id)
    {
        m_Name = name;
        m_ID = id;
        m_Depth = depth;
    }
}

internal class Payload : TreeElement
{
    public float floatValue1, floatValue2, floatValue3;
    public Material material;
    public string text = "";
    public bool enabled;

    public Payload(string name, int depth, int id)
        : base(name, depth, id)
    {
        floatValue1 = Random.value;
        floatValue2 = Random.value;
        floatValue3 = Random.value;
        enabled = true;
    }
}

internal class TreeViewItem<T> : TreeViewItem where T : TreeElement
{
    public T data { get; set; }

    public TreeViewItem(int id, int depth, string displayName, T data)
        : base(id, depth, displayName)
    {
        this.data = data;
    }
}

public class SimpleTreeView : TreeView
{
    const float kToggleWidth = 18f;

    // All columns
    enum MyColumns
    {
        Icon1,
        Icon2,
        Name,
        Value1,
        Value2,
        Value3,
    }

    public enum SortOption
    {
        Name,
        Value1,
        Value2,
        Value3,
    }

    // Sort options per column
    SortOption[] m_SortOptions =
    {
            SortOption.Value1,
            SortOption.Value3,
            SortOption.Name,
            SortOption.Value1,
            SortOption.Value2,
            SortOption.Value3
        };

    List<TreeViewItem> m_Rows = new List<TreeViewItem>(100);

    static Texture2D[] s_TestIcons =
    {
        UnityEditor.EditorGUIUtility.FindTexture ("Folder Icon"),
        UnityEditor.EditorGUIUtility.FindTexture ("AudioSource Icon"),
        UnityEditor.EditorGUIUtility.FindTexture ("Camera Icon"),
        UnityEditor.EditorGUIUtility.FindTexture ("Windzone Icon"),
        UnityEditor.EditorGUIUtility.FindTexture ("GameObject Icon")

    };

    public SimpleTreeView(TreeViewState treeViewState, MultiColumnHeader header)
        : base(treeViewState, header)
    {
        multiColumnHeader.sortingChanged += OnSortingChanged;

        Reload();
    }

    void OnSortingChanged(MultiColumnHeader header)
    {
        SortIfNeeded(rootItem, GetRows());
    }

    void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
    {
        if (rows.Count <= 1)
            return;

        if (multiColumnHeader.sortedColumnIndex == -1)
            return;

        SortByMultipleColumns(rows);
        //TreeToList(root, rows);
        Repaint();
    }

    static void TreeToList(TreeViewItem root, IList<TreeViewItem> result)
    {
        if (root == null)
            throw new NullReferenceException("root");
        if (result == null)
            throw new NullReferenceException("result");

        result.Clear();

        if (root.children == null)
            return;

        Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
        for (int i = root.children.Count - 1; i >= 0; i--)
            stack.Push(root.children[i]);

        while (stack.Count > 0)
        {
            TreeViewItem current = stack.Pop();
            result.Add(current);

            if (current.hasChildren && current.children[0] != null)
            {
                for (int i = current.children.Count - 1; i >= 0; i--)
                {
                    stack.Push(current.children[i]);
                }
            }
        }
    }

    void SortByMultipleColumns(IList<TreeViewItem> rows)
    {
        var sortedColumns = multiColumnHeader.state.sortedColumns;

        if (sortedColumns.Length == 0)
            return;

        var myTypes = rows.Cast<TreeViewItem<Payload>>().ToList();// rootItem.children.Cast<TreeViewItem<Payload>>();
        var orderedQuery = InitialOrder(myTypes, sortedColumns);
        for(int i = 1; i < sortedColumns.Length; ++i)
        {
            SortOption sortOption = m_SortOptions[sortedColumns[i]];
            bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[i]);

            switch (sortOption)
            {
                case SortOption.Name:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.name, ascending);
                    break;
                case SortOption.Value1:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.floatValue1, ascending);
                    break;
                case SortOption.Value2:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.floatValue2, ascending);
                    break;
                case SortOption.Value3:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.floatValue3, ascending);
                    break;
            }
        }

        var temp = orderedQuery.ToList();
        rows.Clear();
        foreach (var row in temp)
        {
            rows.Add(row);
        }
        //rootItem.children = orderedQuery.Cast<TreeViewItem>().ToList();
    }

    IOrderedEnumerable<TreeViewItem<Payload>> InitialOrder(IEnumerable<TreeViewItem<Payload>> myTypes, int[] history)
    {
        SortOption sortOption = m_SortOptions[history[0]];
        bool ascending = multiColumnHeader.IsSortedAscending(history[0]);
        switch (sortOption)
        {
            case SortOption.Name:
                return myTypes.Order(l => l.data.name, ascending);
            case SortOption.Value1:
                return myTypes.Order(l => l.data.floatValue1, ascending);
            case SortOption.Value2:
                return myTypes.Order(l => l.data.floatValue2, ascending);
            case SortOption.Value3:
                return myTypes.Order(l => l.data.floatValue3, ascending);
            default:
                System.Diagnostics.Debug.Assert(false, "Unhandled enum");
                break;
        }

        // default
        return myTypes.Order(l => l.data.name, ascending);
    }

    protected override TreeViewItem BuildRoot()
    {
        var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
        var allItems = new List<TreeViewItem>
            {
                new TreeViewItem {id = 1, depth = 0, displayName = "Animals"},
                new TreeViewItem {id = 2, depth = 0, displayName = "Mammals"},
                new TreeViewItem {id = 3, depth = 0, displayName = "Tiger"},
                new TreeViewItem {id = 4, depth = 0, displayName = "Elephant"},
                new TreeViewItem {id = 5, depth = 0, displayName = "Okapi"},
                new TreeViewItem {id = 6, depth = 0, displayName = "Armadillo"},
                new TreeViewItem {id = 7, depth = 0, displayName = "Reptiles"},
                new TreeViewItem {id = 8, depth = 0, displayName = "Crocodile"},
                new TreeViewItem {id = 9, depth = 0, displayName = "Lizard"},
            };

        // Utility method that initializes the TreeViewItem.children and -parent for all items.
        SetupParentsAndChildrenFromDepths(root, allItems);

        // Return root of the tree
        return root;
    }

    protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
    {
        m_Rows.Clear();
        foreach (TreeViewItem child in root.children)
        {
            var newPayload = new Payload(child.displayName, 0, child.id);
            var item = new TreeViewItem<Payload>(child.id, 0, child.displayName, newPayload);
            m_Rows.Add(item);
        }
        return m_Rows;
    }

    protected override void RowGUI(RowGUIArgs args)
    {
        var item = (TreeViewItem<Payload>)args.item;

        for(int i = 0; i < args.GetNumVisibleColumns(); ++i)
        {
            CellGUI(args.GetCellRect(i), item, (MyColumns)args.GetColumn(i), ref args);
        }
    }

    void  CellGUI(Rect cellRect, TreeViewItem<Payload> item, MyColumns column, ref RowGUIArgs args)
    {
        CenterRectUsingSingleLineHeight(ref cellRect);

        switch(column)
        {
            case MyColumns.Icon1:
                {
                    GUI.DrawTexture(cellRect, s_TestIcons[0], ScaleMode.ScaleToFit);
                }
                break;

            case MyColumns.Icon2:
                {
                    GUI.DrawTexture(cellRect, s_TestIcons[0], ScaleMode.ScaleToFit);
                }
                break;

            case MyColumns.Name:
                {
                    // do toggle
                    Rect toggleRect = cellRect;
                    //toggleRect.x += GetContentIndent(item);
                    toggleRect.width = kToggleWidth;
                    if(toggleRect.xMax < cellRect.xMax)
                    {
                        item.data.enabled = UnityEditor.EditorGUI.Toggle(toggleRect, item.data.enabled);
                    }

                    // default icon and label
                    args.rowRect = cellRect;
                    base.RowGUI(args);
                }
                break;

            case MyColumns.Value1:
            case MyColumns.Value2:
            case MyColumns.Value3:
                {
                    cellRect.xMin += 5f;

                    if(column == MyColumns.Value1)
                    {
                        item.data.floatValue1 = UnityEditor.EditorGUI.Slider(cellRect, UnityEngine.GUIContent.none, item.data.floatValue1, 0f, 1f);
                    }
                    if(column == MyColumns.Value2)
                    {
                        item.data.material = (Material)UnityEditor.EditorGUI.ObjectField(cellRect, UnityEngine.GUIContent.none, item.data.material, typeof(Material), false);
                    }
                    if(column == MyColumns.Value3)
                    {
                        item.data.text = UnityEngine.GUI.TextField(cellRect, item.data.text);
                    }
                }
                break;
        }
    }
}

static class MyExtensionMethods
{
    public static IOrderedEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
    {
        if(ascending)
        {
            return source.OrderBy(selector);
        }
        else
        {
            return source.OrderByDescending(selector);
        }
    }

    public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
    {
        if(ascending)
        {
            return source.ThenBy(selector);
        }
        else
        {
            return source.ThenByDescending(selector);
        }
    }
}
