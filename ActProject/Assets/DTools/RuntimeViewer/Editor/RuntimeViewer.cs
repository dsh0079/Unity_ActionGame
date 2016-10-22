using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class RuntimeViewer :  EditorWindow
{
    /*TODO: RuntimeViewer
     * 
     * 1. 加上屏蔽系统组件功能 (ok)
     * 2. 刷新后也保持原来的打开关闭状态
     * 3. 支持实时更新           (以后再说)
     * 4. 在编辑状态下强制关闭一些会报错的系统组件(ok)
     * 5. 处理集合, 数组,list,字典之类的不能单纯的当做引用类型来处理.(ok)
     * 6. 显示字段的 public or private or protected
     * 
     */


    readonly string _ver = "2015.04.19 _ver 0.02";
    readonly int space_vertical = 8;

    //public bool IsRealtimeUpdate = false;
    public static bool IsForbidSystemProperty = true;

    Vector2 scrollPosition = Vector2.zero;
    GameObject nowSelectItem;

    int nowSelectItemID=-1;
    //对应nowSelectItem上所有脚本
    List<RVCollection> rvCollections = new List<RVCollection>();

    void Awake()
    {
    }

    //绘制窗口时调用
    void OnGUI()
    {
        GUILayout.Space(space_vertical);
        GUILayout.Label(_ver);
        GUILayout.Space(space_vertical);

        // IsRealtimeUpdate = EditorGUILayout.Toggle("Is Open Realtime Update", IsRealtimeUpdate);
        bool b = IsForbidSystemProperty;
        b = EditorGUILayout.Toggle("Forbid System Propertys", b);
        if(IsForbidSystemProperty != b)
        {
            Refresh();
        }
        IsForbidSystemProperty = b;

        if (GUILayout.Button("-- Refresh --", GUILayout.Width(200)))
        {
            Refresh();
        }
        GUILayout.Space(space_vertical);

        nowSelectItem = null;
        foreach (Transform t in Selection.transforms)
        {
            if (t != null && t.gameObject != null)
            {
                nowSelectItem = t.gameObject;
                break;
            }
        }

        if (nowSelectItem != null)
        {
            GUILayout.Label("Select Item : " + nowSelectItem.name);
        }
        else
        {
            GUILayout.Label("Select Item : is null");
        }
        GUILayout.Space(space_vertical);

        if (nowSelectItem != null)
        {
            //切换物体
            if (nowSelectItemID != nowSelectItem.GetInstanceID())
            {
                rvCollections = CreateRVCollections();
                nowSelectItemID = nowSelectItem.GetInstanceID();
                scrollPosition = Vector2.zero;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            //             if (IsRealtimeUpdate == true)
            //             {
            //                 foreach (var item in rvCollections)
            //                 {
            //                     item.OnGUIUpdate(nowSelectItem);
            //                 }
            //             }
            //             else
            //             {
            foreach (var item in rvCollections)
            {
                item.OnGUIUpdate(null);
            }
            //  }
            EditorGUILayout.EndScrollView();
        }
    }

    private void Refresh()
    {
        if (nowSelectItem != null)
        {
            rvCollections = CreateRVCollections();
            nowSelectItemID = nowSelectItem.GetInstanceID();
        }
    }

    //当窗口关闭时调用
    void OnDestroy()
    {
        if (rvCollections != null && rvCollections.Count>0)
        {
            foreach (var item in rvCollections)
            {
                item.OnDestroy();
            }
        }
    }

    List<RVCollection> CreateRVCollections()
    {
        List<RVCollection> result = new List<RVCollection>();

        if(nowSelectItem != null)
        {
            Component[] c = RVHelper.GetComponent(nowSelectItem);
            foreach (var item in c)
            {
                RVCollection rvc = new RVCollection(item, 0, false);
                rvc.NameLabel = item.GetType().ToString().Replace("UnityEngine.", "");
                result.Add(rvc);
            }
        }
        return result;
    }

    #region ...

    //更新
    void Update()
    {

    }

    //当窗口获得焦点时调用一次
    void OnFocus()
    {
    }

    //当窗口丢失焦点时调用一次
    void OnLostFocus()
    {
    }
    void OnInspectorUpdate()
    {
        this.Repaint();
    }


    //void OnHierarchyChange()
    //{
    //    Debug.Log("当Hierarchy视图中的任何对象发生改变时调用一次");
    //}

    //void OnProjectChange()
    //{
    //    Debug.Log("当Project视图中的资源发生改变时调用一次");
    //}

    //void OnInspectorUpdate()
    //{
    //    this.Repaint();
    //}





    [MenuItem("Window/RuntimeViewer")]
    static void AddWindow()
    {
        //创建窗口
        RuntimeViewer window = EditorWindow.GetWindow<RuntimeViewer>(false, "RuntimeViewer", true);
        window.Show();
    }

    #endregion
}
