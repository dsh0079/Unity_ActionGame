using UnityEngine;
using System.Collections;

 /// <summary>
/// 快速设置界面变量,加到DTool脚本所在的GameObject上就可以使用了
 /// </summary>
public class InspectorField : MonoBehaviour
{
    public static InspectorField Values;

    //游戏的FPS 
    public int TargetFPS = 300;

    public bool IsShowFPS = true;
    public bool IsPlayBGM = true;
    public bool IsPlaySound = true;

    public float ShootGroundHitPointHeightOffest = 1f;
    public float ShootModeRotLerp = 0.3f;
    public float ShootGroundHitPointVainDir = 1f;

}
