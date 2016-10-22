using UnityEngine;
using System.Collections;
using DTool;
 
 /// <summary>
 /// ver 1.00 2014.5.31 :
 /// 包含各种快速方便的工具,
 /// 分为测试类和非测试类, 测试类可以一键关闭.
 /// 使用时把这个类加在一个不会销毁的GameObject上
 /// 
 /// ---------------------------
 /// ver 1.10 2014.9.8 :
 /// 增加 Editor和UnityVS 文件夹, 增加EventManager
 /// ---------------------------
 /// ver 1.20 2015.1.1 :
 /// 重构, 不用通过DTools.来调用了...改为直接调用各个类, DTools负责管理初始化,更新和销毁
 /// 
 /// </summary>
public class DTools : MonoBehaviour
{
    /// <summary>
    /// 是否使用测试类的工具
    /// </summary>
    public bool IsOpenTest = true;
    public Color ScreenOutputFontColor = Color.green;

    ShowFPS showFPS;

    /// <summary>
    /// 是否显示正在执行的AutoTimer方法
    /// </summary>
    public bool IsShowAutoTimerNames = false;
    public string[] autoTimerNames;

    /// <summary>
    /// 按住回车时的加速倍数
    /// </summary>
    public float AccelerationMultiple = 5f;

    /// <summary>
    /// 正常的速度
    /// </summary>
    public float NormalSpeed= 1f;


    /// <summary>
    /// 显示信息是位置
    /// </summary>
    public float ShowMessageY = 0f;

    void Awake()
    {
        ShowMessage.rect = new Rect(0, 0, 800, 52);
        ShowMessage.style = new GUIStyle();
        ShowMessage.style.normal = new UnityEngine.GUIStyleState();
        ShowMessage.style.normal.textColor = ScreenOutputFontColor;
        showFPS = new ShowFPS();

        InspectorField.Values = this.gameObject.GetComponent<InspectorField>();
    }

    void Update()
    {
        if (IsOpenTest == true)
        {
            ShowMessage.Update();
            showFPS.Update();
            VisualTest.Update();
        }

        AutoTimer.Update();

        if (IsShowAutoTimerNames == true)
        {
            autoTimerNames = AutoTimer.GetAllName();
        }

         
            if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = AccelerationMultiple;
            }
            else
            {
                Time.timeScale = NormalSpeed;
            }
        

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            UnityEditor.EditorApplication.isPaused = true;
        }
#endif
    }

    void OnGUI()
    {
        if (IsOpenTest == true)
        {
            ShowMessage.OnGUI( ShowMessageY );
        }
    }

    void OnDrawGizmos()
    {
        if (IsOpenTest == true)
        {
            VisualTest.OnDrawGizmos();
        }
    }

    /// <summary>
    /// 场景销毁时调用,注册到场景销毁事件中去
    /// </summary>
    public static void OnDestroyEvent()
    {
        //QuickSave.SaveToDisk();
        ShowMessage.ClearAll();
        VisualTest.ClearAll();
    }
}
