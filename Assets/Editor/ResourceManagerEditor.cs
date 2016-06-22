using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ResourceManagerScript))]
public class ResourceManagerEditor : Editor
{

    private List<GameElement> currResList;

    public override void OnInspectorGUI()
    {
        ResourceManagerScript myTarget = (ResourceManagerScript)target;
        currResList = myTarget.currResList;

        foreach (var res in currResList)
        {
            res.value =  EditorGUILayout.IntField(res.name, res.value);
            res.enabled = EditorGUILayout.Toggle(res.name, res.enabled);
        }
    }
}
