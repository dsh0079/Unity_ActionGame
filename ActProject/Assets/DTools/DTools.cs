using UnityEngine;
using System.Collections;
using DTool;
 
 /// <summary>
 /// ver 1.00 2014.5.31 :
 /// �������ֿ��ٷ���Ĺ���,
 /// ��Ϊ������ͷǲ�����, ���������һ���ر�.
 /// ʹ��ʱ����������һ���������ٵ�GameObject��
 /// 
 /// ---------------------------
 /// ver 1.10 2014.9.8 :
 /// ���� Editor��UnityVS �ļ���, ����EventManager
 /// ---------------------------
 /// ver 1.20 2015.1.1 :
 /// �ع�, ����ͨ��DTools.��������...��Ϊֱ�ӵ��ø�����, DTools��������ʼ��,���º�����
 /// 
 /// </summary>
public class DTools : MonoBehaviour
{
    /// <summary>
    /// �Ƿ�ʹ�ò�����Ĺ���
    /// </summary>
    public bool IsOpenTest = true;
    public Color ScreenOutputFontColor = Color.green;

    ShowFPS showFPS;

    /// <summary>
    /// �Ƿ���ʾ����ִ�е�AutoTimer����
    /// </summary>
    public bool IsShowAutoTimerNames = false;
    public string[] autoTimerNames;

    /// <summary>
    /// ��ס�س�ʱ�ļ��ٱ���
    /// </summary>
    public float AccelerationMultiple = 5f;

    /// <summary>
    /// �������ٶ�
    /// </summary>
    public float NormalSpeed= 1f;


    /// <summary>
    /// ��ʾ��Ϣ��λ��
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
    /// ��������ʱ����,ע�ᵽ���������¼���ȥ
    /// </summary>
    public static void OnDestroyEvent()
    {
        //QuickSave.SaveToDisk();
        ShowMessage.ClearAll();
        VisualTest.ClearAll();
    }
}
