using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpinControlAsset))]
public class SpinControlAssetEditor : Editor
{
    private SpinControlAsset m_target;
    private List<string> propertyNameList = new List<string>();
    private int propertyIndex = 0;

    private void OnEnable()
    {
        m_target = (SpinControlAsset)target;
        propertyNameList.Add("Transform.Position");
        propertyNameList.Add("Transform.Rotation");
        propertyNameList.Add("Transform.Scale");
    }

    private void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        propertyIndex = EditorGUILayout.Popup(propertyIndex, propertyNameList.ToArray());
        var propertyName = propertyNameList[propertyIndex];
        if (propertyName == "Transform.Position")
        {
            m_target.position = EditorGUILayout.Vector3Field(propertyName, m_target.position);
        }
    }
}
