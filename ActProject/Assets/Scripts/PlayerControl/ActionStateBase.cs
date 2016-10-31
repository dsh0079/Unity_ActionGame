using UnityEngine;
using System.Collections;

public class ActionStateBase
{
    public ActionState State { private set; get; }
    protected ActionMachine am { private set; get; }
    protected PlayerCharater chara { get { return am.Charater as PlayerCharater; } }

    public ActionStateBase(ActionState state, ActionMachine am)
    {
        this.am = am;
        this.State = state;
    }

    public virtual void Init()
    {

    }

    public virtual void Enter()
    {

    }

    //已经处于此状态时, 外部再要求进入时会调用,与 IsCanTransform() 没有关系
    //只要外包要求,就会调用
    public virtual void ReEnter()
    {

    }

    public virtual void Update()
    {


    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Quit()
    {

    }



    public virtual bool IsCanTransform()
    {
        return true;
    }
}
