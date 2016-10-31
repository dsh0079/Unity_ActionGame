using UnityEngine;
using System.Collections;

/// <summary>
/// 移动有关的东西都写在这里，开放按键后并不会立即结束这个状态，
/// 状态之间添加一个过去期！！
/// </summary>
public class MoveAction : ActionStateBase
{
    float nowSpeed;

    public MoveAction(ActionMachine am)
        : base(ActionState.Move, am)
    {
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        Vector3 dir = PlayerInput.Instance.InputDirecton;
        if (dir == Vector3.zero)
            return;

        float newSpeed = this.chara.MoveSpeed;

        if (PlayerInput.Instance.IsOn(PInput.Run))
            newSpeed *= this.chara.RunSpeedRate;

        nowSpeed = Mathf.Lerp(nowSpeed, newSpeed, this.chara.MoveSpeedAccLerp);

        this.chara.SetPosition(dir * nowSpeed * Time.deltaTime);

        ShowMessage.Add("MoveAction->speed", nowSpeed);
    }
}
