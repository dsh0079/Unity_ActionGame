using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用于在屏幕上输出调试信息.
/// </summary>
public class ShowMessage
{
    static List<string> messages = new List<string>();
    static List<string> names = new List<string>();

    public static GUIStyle style = null;
    public static Rect rect;

    public static float IntervalSize = 16;
    //绘制持续时间(秒)
    public static float ClearTime = 0.6f;
    static float nowTime = 0;

    public static void Update()
    {
        if (nowTime < ClearTime)
            nowTime += Time.deltaTime;
        else
        {
            messages.Clear();
            names.Clear();
            nowTime = 0;
        }
    }

    public static void OnGUI(float showMessageY)
    {
        Display(showMessageY);
    }

    static void Display(float showMessageY)
    {
        for (int i = 0; i < names.Count; i++)
        {
            GUI.Box(new Rect(0, i * IntervalSize + showMessageY, rect.width, rect.height),
                names[i] + " : " + messages[i], style);
        }

    }

    public static void Add(string name, string message)
    {
        if (names.Contains(name) == false)
        {
            names.Add(name);
            messages.Add(message);
        }
        else
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i] == name)
                {
                    messages[i] = message;
                    break;
                }
            }
        }
    }

    public static void Add(string name, object mess)
    {
        string message = mess.ToString();
        Add(name, message);
    }

    public static void Add(string name, bool mess)
    {
        string message;

        if (mess == true)
            message = mess.ToString() + "~~~~~~~";
        else
            message = mess.ToString() + ".....";

        Add(name, message);
    }

    public static void ClearAll()
    {
        messages.Clear();
        names.Clear();
    }
}
