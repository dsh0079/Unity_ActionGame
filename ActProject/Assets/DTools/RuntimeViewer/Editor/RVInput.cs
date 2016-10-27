using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;

public class RVInput
{
    object data;
    RVControlBase parent;
    RVSettingData settingData;
    RVVisibility rvVisibility;
    RVNoParameterDel onClose;
    string nameLabel;
    string namePath;
    RuntimeViewer rv;

    public RVInput(RuntimeViewer rv, string nameLabel, string namePath, object data, RVControlBase parent, RVSettingData settingData, RVVisibility rvVisibility,
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
        try
        {
            EditorGUILayout.BeginHorizontal();

            if (rvVisibility.ValueIsString() == true)
            {
                data = Intput_String(data);
            }
            else if (rvVisibility.ValueIsNumeric() == true)
            {
                data = Intput_Numeric(data);
            }
            else if (rvVisibility.ValueIsEnum() == true)
            {
                data = Intput_Enum(data);
            }
            else if (rvVisibility.ValueIsBool() == true)
            {
                data = Intput_Bool(data);
            }
            else if (rvVisibility.ValueIsVector() == true)
            {
                data = Intput_Vector(data);
            }
            else if (rvVisibility.ValueIsColor() == true)
            {
                data = Intput_Color(data);
            }
            else if (this.data == null)
            {
                data = Intput_New(data);
            }
            else
            {
                EditorGUILayout.LabelField(new GUIContent("error : not support ..."));
            }

            Rect rect = GUILayoutUtility.GetLastRect();
            rect.y -= 1;
            rect.x += rect.width + 5;
            rect.width = 43;
            rect.height = 17;
            if (GUILayout.Button("Apply", GUILayout.Width(43)))
            {
                onClose();
                OnSetValue(data, rvVisibility);
            }
            if (GUILayout.Button("Cancel", GUILayout.Width(53)))
            {
                Cancel();
            }
            EditorGUILayout.EndHorizontal();
        }
        catch (Exception e)
        {
            string dataStr = "null";
            if (data != null)
                dataStr = data.ToString();

            ErrorLog("can't set value '"+ dataStr + "' to '" + this.nameLabel + "'   path>>" + this.namePath + "", e);
            Cancel();
        }
    }

    void OnSetValue(object newValue, RVVisibility rvVisibility)
    {
        if (rvVisibility == null)
        {
            ErrorLog("RVVisibility == null ...", null);
            return;
        }

        object tagetTypeValue = null;

        if (newValue != null)
        {
            tagetTypeValue = Convert.ChangeType(newValue, this.rvVisibility.ValueType);
        }
        else if (RVHelper.IsUnityClass(this.rvVisibility.ValueType))
        {
            ErrorLog("'" + this.rvVisibility.ValueType.ToString() + "' instance of 'UnityEngine.Object/MonoBehaviour' can not be created...", null);
            return;
        }
        else
        {// 创建新的class or集合
            tagetTypeValue = this.rvVisibility.ValueType.Assembly.CreateInstance(this.rvVisibility.ValueType.FullName);
            this.rv.RefreshAtEndOfFrame();
        }
     
        if (rvVisibility.RVType == RVVisibility.NameType.Field)
        {
            rvVisibility.FieldInfo.SetValue(rvVisibility.ParentData, tagetTypeValue);
        }
        else if (rvVisibility.RVType == RVVisibility.NameType.Property)
        {
            rvVisibility.PropertyInfo.SetValue(rvVisibility.ParentData, tagetTypeValue, null);
        }
        else
        {
            ErrorLog("NameType is not Field/Property ...", null);
        }
    }

    void ErrorLog(string str, Exception e)
    {
        if (e != null)
            Debug.LogError("RuntimeViewer ChangeValue Failure : " + str + " \n| error: " + e.Message + " |  " + e.StackTrace);
        else
            Debug.LogError("RuntimeViewer ChangeValue Failure : " + str + "\n");
    }

    object Intput_String(object obj)
    {
        string str = "";
        if (obj != null)  //nullに設定できない！
            str = obj.ToString();

        str = EditorGUILayout.TextField(str, GUILayout.MaxWidth(180), GUILayout.MinWidth(10));
        return str;
    }

    //nullかないの数　int,floatなと
    object Intput_Numeric(object obj)
    {
        if (obj == null)
        {
            Debug.LogError(" RVInput.Intput_Numeric() error : data is null... ");
            return false;
        }

        if (this.rvVisibility.ValueIsFloat() == true)
        {
            float _float = Convert.ToSingle(obj);
            return EditorGUILayout.FloatField(_float, GUILayout.MaxWidth(180), GUILayout.MinWidth(10));
        }
        else if (this.rvVisibility.ValueIsDouble() == true)
        {
            double _double = Convert.ToDouble(obj);
            return EditorGUILayout.DoubleField(_double, GUILayout.MaxWidth(180), GUILayout.MinWidth(10));
        }
        else if (this.rvVisibility.ValueIsLong() == true)
        {
            long _long = Convert.ToInt64(obj);
            return EditorGUILayout.LongField(_long, GUILayout.MaxWidth(180), GUILayout.MinWidth(10));
        }
        else
        {
            int _int = Convert.ToInt32(obj);
            _int = EditorGUILayout.IntField(_int, GUILayout.MaxWidth(180), GUILayout.MinWidth(10));
            return _int;
            //return Convert.ChangeType(_int, this.rvVisibility.ValueType);
        }
    }

    object Intput_Enum(object obj)
    {
        if (obj == null)
        {
            Debug.LogError(" RVInput.Intput_Numeric() error : data is null... ");
            return false;
        }

        Enum _enum = (Enum)Convert.ChangeType(obj, this.rvVisibility.ValueType);
        _enum = EditorGUILayout.EnumPopup(_enum);
        return _enum;
    }

    object Intput_Bool(object obj)
    {
        if (obj == null)
        {
            Debug.LogError(" RVInput.Intput_Numeric() error : data is null... ");
            return false;
        }

        int select = (bool)Convert.ChangeType(obj, typeof(bool)) == true ? 1 : 0;
        select = EditorGUILayout.IntPopup(select, new GUIContent[] { new GUIContent("True"), new GUIContent("False") }
        , new int[] { 1, 0 }, GUILayout.Width(120));

        return select == 1 ? true : false;
    }

    object Intput_Vector(object obj)
    {
        if (obj == null)
        {
            Debug.LogError(" RVInput.Intput_Numeric() error : data is null... ");
            return false;
        }

        if (typeof(Vector2).IsAssignableFrom(this.rvVisibility.ValueType) == true)
        {
            return EditorGUILayout.Vector2Field("", (Vector2)Convert.ChangeType(obj, typeof(Vector2)));
        }
        else if (typeof(Vector3).IsAssignableFrom(this.rvVisibility.ValueType) == true)
        {
            return EditorGUILayout.Vector3Field("", (Vector3)Convert.ChangeType(obj, typeof(Vector3)));
        }
        else if (typeof(Vector4).IsAssignableFrom(this.rvVisibility.ValueType) == true)
        {
            return EditorGUILayout.Vector4Field("   ", (Vector4)Convert.ChangeType(obj, typeof(Vector4)));
        }

        return obj;
    }

    object Intput_Color(object obj)
    {
        if (obj == null)
        {
            Debug.LogError(" RVInput.Intput_Numeric() error : data is null... ");
            return false;
        }

        try
        {
            if (typeof(Color).IsAssignableFrom(this.rvVisibility.ValueType) == true)
            {
                return EditorGUILayout.ColorField((Color)Convert.ChangeType(obj, typeof(Color)), GUILayout.Width(120));
            }
            else if (typeof(Color32).IsAssignableFrom(this.rvVisibility.ValueType) == true)
            {
                return (Color32)EditorGUILayout.ColorField((Color32)Convert.ChangeType(obj, typeof(Color32)), GUILayout.Width(120));
            }
        }
        catch
        {
            return obj;
        }

        return obj;
    }

    object Intput_New(object obj)
    {
        EditorGUILayout.LabelField(new GUIContent("click apply to create new instance"), GUILayout.Width(198));
        return obj;
    }

    void Cancel()
    {
        onClose();
    }
}
