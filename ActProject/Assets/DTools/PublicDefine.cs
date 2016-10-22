using UnityEngine;
using System.Collections;

/// <summary>
/// 一些公用的定义, 委托 ,枚举之类的
/// </summary>


public delegate void NoParameterDel();
public delegate void OneParameterDel(object obj);
public delegate void BoolParameterDel(bool b);
public delegate void GOParameterDel(GameObject obj);
public delegate object ReturnObjDel();
public delegate bool ReturnBoolDel();
public delegate void StrParameterDel(string str);
public delegate void TwoFloatParameterDel(float f1, float f2);
public delegate void FloatParameterDel(float f1);
public delegate void IntParameterDel(int _int);


public static class ExtendMethod
{
    public static T FindChild<T>(this Transform tra, string childName) where T : Component
    {
        Transform child = tra.FindChild(childName);
        if (child == null)
        {
            Debug.LogError(tra.name + " don't had child name is '" + childName + "' ...");
            return null;
        }
        else
        {
            return child.GetComponent<T>();
        }
    }
}