using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class RVControlBase
{
    public int ID = -1; //只用于第一层,RuntimeViewer脚本中
    public readonly int depth = 0;

    public string NameLabel ="error";
    protected object data = null;

    //层缩进长度,其实值为 Indent * depth
    public static readonly int Indent_class = 5;

    public static readonly int Indent_field = 5;

    public RVControlBase(object data, int depth)
    {
        this.data = data;
        this.depth = depth;
    }

    public virtual void OnGUIUpdate(object data)
    {
    }

    public virtual void OnDestroy()
    {

    }

}
