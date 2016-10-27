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
        if (GUILayout.Button("Add Item", GUILayout.Width(43)))
        {
            onClose();
            OnSetValue(data, rvVisibility);
        }
        if (GUILayout.Button("Cancel", GUILayout.Width(53)))
        {
            onClose();
        }
    }

    void OnSetValue(object newValue, RVVisibility rvVisibility)
    {
        if (rvVisibility == null)
        {
            ErrorLog("RVVisibility == null ...", null);
            return;
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
