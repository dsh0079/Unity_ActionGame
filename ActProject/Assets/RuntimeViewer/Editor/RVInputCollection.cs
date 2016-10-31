using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;

public class RVInputCollection
{
    object data;
    RVControlBase parent;
    RVSettingData settingData;
    RVVisibility rvVisibility;
    RVNoParameterDel onClose;
    string nameLabel;
    string namePath;
    RuntimeViewer rv;

    public RVInputCollection(RuntimeViewer rv, string nameLabel, string namePath, object data, RVControlBase parent, RVSettingData settingData, RVVisibility rvVisibility,
       RVNoParameterDel onClose)
    {
        this.rv = rv;
        this.namePath = namePath;
        this.nameLabel = nameLabel;
        this.data = data;
        this.parent = parent;
        this.settingData = settingData;
        this.rvVisibility = rvVisibility;
        this.onClose = onClose;
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        Type t = data.GetType();
        if (typeof(IDictionary).IsAssignableFrom(t) == true)//是个字典
        {

        }
        else if (typeof(ICollection).IsAssignableFrom(t) == true) //是个集合
        {
            AddItem_List();
        }

        if (GUILayout.Button("Cancel", GUILayout.Width(56)))
        {
            onClose();
        }

        EditorGUILayout.EndHorizontal();

    }

    void OnSetValue(object newValue, RVVisibility rvVisibility)
    {
        if (rvVisibility == null)
        {
            ErrorLog("RVVisibility == null ...", null);
            return;
        }
    }
    
    void AddItem_List()
    {
        EditorGUILayout.LabelField("   add one item with default value ? ", GUILayout.Width(206));

        if (GUILayout.Button("Add", GUILayout.Width(66)))
        {
            onClose();
            OnSetValue(data, rvVisibility);
        }
    }

    void ErrorLog(string str, Exception e)
    {
        if (e != null)
            Debug.LogError("RuntimeViewer ChangeCollection Failure : " + str + " \n| error: " + e.Message + " |  " + e.StackTrace);
        else
            Debug.LogError("RuntimeViewer ChangeCollection Failure : " + str + "\n");
    }
}
