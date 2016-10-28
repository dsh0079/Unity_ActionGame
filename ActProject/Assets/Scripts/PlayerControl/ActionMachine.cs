using UnityEngine;
using System.Collections;

public enum ActionState
{
    Idle,
    Move,
    Jump,
    Roll,
}

public class ActionMachine
{
    public ActionStateBase NowState { protected set; get; }
    public CharaterBase Charater { protected set; get; }

    public ActionMachine(CharaterBase cb)
    {
        this.Charater = cb;
        this.Charater.OnDeadEvent += OnDeath;
        NowState = new IdleAction(this);
        NowState.Init();
        NowState.Enter();
    }

    public void AMUpdate()
    {
        if (PlayerInput.Instance.IsOn(PInput.Jump))
        {
            ChangeState(ActionState.Jump);
        }
        else if(PlayerInput.Instance.IsOn(PInput.Roll))
        {
            ChangeState(ActionState.Roll);
        }
        else if (PlayerInput.Instance.InputDirecton != Vector3.zero)
        {
            ChangeState(ActionState.Move);
        }
        else
        {
            ChangeState(ActionState.Idle);
        }

        if (NowState != null)
            NowState.Update();
    }

    void ChangeState(ActionState aState)
    {
        if (NowState == null)
            return;

        if (NowState.State == aState)
        {
            NowState.ReEnter();
            return;
        }

        if (NowState.IsCanTransform() == false)
            return;

        ActionStateBase newState = GetNewState(aState);

        if (newState != null)
        {
            NowState.Quit();
            NowState = newState;
            NowState.Init();
            NowState.Enter();
        }
        else
        {
            Debug.LogError("ActionMachine -> ChangeState() : not exist state name  is " + aState.ToString());
        }
    }

    public void FixedUpdate()
    {
        if (NowState == null)
            return;

        NowState.FixedUpdate();
    }

    protected virtual ActionStateBase GetNewState(ActionState state)
    {
        switch (state)
        {
            case ActionState.Idle:
                return new IdleAction(this);
            case ActionState.Jump:
                return new JumpAction(this);
            case ActionState.Move:
                return new MoveAction(this);
        }
        return null;
    }

    protected void OnDeath(LiveObject lo)
    {
        if (NowState != null)
        {
            NowState.Quit();
            NowState = null;
        }
    }

}

