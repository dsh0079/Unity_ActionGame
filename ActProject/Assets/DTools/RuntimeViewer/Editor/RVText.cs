using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class RVText : RVControlBase
{
    public RVText(object data, int depth)
        : base(data, depth)
    {
    }

    public override void OnGUIUpdate(object data)
    {
        if(data != null)
        {
            this.data = data;
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("     ", GUILayout.Width(depth * RVControlBase.Indent_field));
        if (this.data != null)
        {
            EditorGUILayout.LabelField(this.NameLabel + " : " + this.data.ToString());
        }
        else
        {
            EditorGUILayout.LabelField(this.NameLabel + " : [null] ");
        }

        EditorGUILayout.EndHorizontal();
    }

    public override void OnDestroy()
    {
    }
}
