using UnityEngine;
using System.Collections;

public class MoveAction : ActionStateBase
{
    public MoveAction(ActionMachine am)
        : base(ActionState.Move, am)
    {
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        if (PlayerInput.Instance.IsOn(PInput.Run))
            chara.Move(PlayerInput.Instance.InputDirecton, true);
        else
            chara.Move(PlayerInput.Instance.InputDirecton, false);
    }
}
