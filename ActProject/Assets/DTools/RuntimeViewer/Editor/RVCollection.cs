using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;

public class RVCollection : RVControlBase
{
    //Dictionary<string, RVControlBase> children = new Dictionary<string, RVControlBase>();
    List<RVControlBase> children = new List<RVControlBase>();

    protected bool isOpen = false;
    bool isFristOpen = true;

    public RVCollection(object data, int depth, bool isOpen)
        : base(data, depth)
    {
        this.isOpen = isOpen;
        if (isOpen == true)
        {
            AnalyzeAndAddChildren();
        }
    }

    public override void OnGUIUpdate(object _newData)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("     ", GUILayout.Width(depth * RVControlBase.Indent_class));

        EditorGUILayout.BeginVertical();
        isOpen = EditorGUILayout.Foldout(isOpen, NameLabel);
        if (isOpen == true)
        {
            //第一次打开时解析
            if (isFristOpen == true)
            {
                AnalyzeAndAddChildren();
                isFristOpen = false;
            }
            else
            {
                //每帧更新
                if (_newData != null)
                {
                    this.data = _newData;
                    AnalyzeAndAddChildren();
                }
            }

            foreach (var item in children)
            {
                item.OnGUIUpdate(_newData);
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    public override void OnDestroy()
    {
        foreach (var item in children)
        {
            //item.Value.OnDestroy();
            item.OnDestroy();
        }
        children.Clear();
    }

    protected virtual void AnalyzeAndAddChildren()
    {
        OnDestroy();
        if (data == null)
        {
            return;
        }

        Type t = data.GetType();

        if (IsCollection(t) == true)
        {
            children.AddRange(AnalyzeCollection(data, t));
        }
        else
        {
            children.AddRange(AnalyzeClass(data, t));
        }
    }

    RVControlBase CreateControl(object ob, string fieldName)
    {
        RVControlBase b = null;

        if (IsNull(ob) == true)
        {
            b = new RVText(null, depth + 1);
            b.NameLabel = fieldName;
        }
        else
        {
            Type t = ob.GetType();
            if (t.IsValueType == true)
            {
                b = new RVText(ob, depth + 1);
                b.NameLabel = fieldName;
            }
            else
            {
                b = TestString(ob, fieldName); //字符串特殊判断
                if (b != null)
                    return b;
                b = new RVCollection(ob, depth + 1, false);
                b.NameLabel = GetSpecialNameLabel(ob, fieldName);
            }
        }

        return b;
    }

    bool IsForbidThis(object obj, string fieldName)
    {
        if (obj == null)
            return false;

        if (Application.isPlaying == false)
        {
            if (obj is MeshFilter)
                return true;
            if (obj is Renderer)
                return true;
        }

        if (RuntimeViewer.IsForbidSystemProperty == true)
        {
            if (obj is Component)
            {
                if (ForbidSystemProperty.ComponentPropertys.Contains(fieldName) == true)
                    return true;
            }
            else if (obj is GameObject)
            {
                if (ForbidSystemProperty.GameObjectPropertys.Contains(fieldName) == true)
                    return true;
            }
        }
        return false;
    }

    string GetSpecialNameLabel(object ob, string fieldName)
    {
        if (ob == null)
        {
            return fieldName;
        }
        else if (ob is UnityEngine.Object && (ob as UnityEngine.Object) != null)
        {
            fieldName += " : " + (ob as UnityEngine.Object).name;
        }
        else if (IsCollection(ob.GetType()) == true)
        {
            if (typeof(IDictionary).IsAssignableFrom(ob.GetType()) == true)
                fieldName += " : <dictionary>";
            else
                fieldName += " : <collection>";
        }

        return fieldName;
    }

    RVControlBase TestString(object ob, string fieldName)
    {
        RVControlBase b = null;
        if (ob is string)
        {
            b = new RVText(ob, depth + 1);
            b.NameLabel = fieldName;
        }
        return b;
    }

    bool IsNull(object ob)
    {
        if (ob == null)
        {
            return true;
        }
        else if (ob is UnityEngine.Object && (ob as UnityEngine.Object) == null)
        {
            return true;
        }

        return false;
    }

    //是否是个字典 or 集合
    bool IsCollection(Type type)
    {
        if (typeof(IDictionary).IsAssignableFrom(type) == true ||
            typeof(ICollection).IsAssignableFrom(type) == true)
        {
            return true;
        }
        return false;
    }

    //所有集合判断,数组,字典,list等等
    List<RVControlBase> AnalyzeCollection(object ob, Type type)
    {
        List<RVControlBase> result = new List<RVControlBase>();

        if (typeof(IDictionary).IsAssignableFrom(type) == true)//是个字典
        {

            IDictionary dic = ob as IDictionary;
            foreach (DictionaryEntry item in dic)
            {
                string _key = "null";
            //    string _value = "null";

                if (IsNull(item.Key) == false)
                    _key = item.Key.ToString();
            //    if (IsNull(item.Value) == false)
              //      _value = item.Value.ToString();

                result.Add(CreateControl(item.Value, "[" + _key + "]"));
            }
        }
        else if (typeof(ICollection).IsAssignableFrom(type) == true) //是个集合
        {
            foreach (var _v in (ICollection)ob)
            {
                string str = "item";
                if (IsNull(_v) == false)
                    str = _v.ToString();
                result.Add(CreateControl(_v, str));
            }
        }

        return result;
    }

    List<RVControlBase> AnalyzeClass(object data, Type t)
    {
        List<RVControlBase> result = new List<RVControlBase>();

        FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        foreach (FieldInfo field in fields)
        {
            if (IsForbidThis(data, field.Name) == true)
            {
                continue;
            }
            object value = field.GetValue(data);
            result.Add(CreateControl(value, field.Name));
        }

        PropertyInfo[] properties = t.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            object value = null;
            try
            {
                if (IsForbidThis(data, property.Name) == true)
                {
                    continue;
                }
                value = property.GetValue(data, null);
            }
            catch
            {
                value = null;
            }
            result.Add(CreateControl(value, property.Name));
        }
        //
        //result.Sort((a, b) =>
        //{
        //    return string.Compare(a.NameLabel, b.NameLabel, false, System.Globalization.CultureInfo.InvariantCulture);
        //});

        return result;
    }
}
