using UnityEngine;
using System.Collections;
using DTool;

public class PlayerCharater : CharaterBase
{
    //    public float MaxMoveSpeed = 28f;
    //    public float MoveSpeedAcc = 50f;
    //    public float MoveSpeedDec = 28f;

    //    public float RunSpeedRate = 1.6f;
    //    public float MoveVelocity_RotLerp = 0.5f;

    //    public Vector3 NowMoveVelocity { private set; get; }
    //    //public Vector3 NowMoveDir { private set; get; }
    //    public float NowSpeed { private set; get; }


    public float MoveSpeed = 20f;
    public float RunSpeedRate = 1.6f;
    public float MoveSpeedAccLerp = 0.1f;

    CharacterController charaController;

    public override void Init(int ID, int HP, Vector3 pos)
    {
        base.Init(ID, HP, pos);
        charaController = this.GetComponent<CharacterController>();
    }

    public override void LUpdate()
    {
        //return;
        //if (NowSpeed > 0)
        //{
        //    //     charaController.Move(NowMoveDir * NowSpeed * Time.deltaTime);
        //    charaController.Move(NowMoveVelocity * Time.deltaTime);

        //    float speed = NowMoveVelocity.magnitude;
        //    speed -= MoveSpeedDec * Time.deltaTime;
        //    if (speed < 0)
        //        speed = 0;
        //    NowMoveVelocity *= speed;
        //}

        //ShowMessage.Add("speed", NowSpeed);

        base.LUpdate();
    }

    //public void Move(Vector3 newDir, bool isRun)
    //{
    //    return;

    //    if (isRun == true)
    //    {
    //        NowSpeed += MoveSpeedAcc * RunSpeedRate * Time.deltaTime;
    //        if (NowSpeed > MaxMoveSpeed * RunSpeedRate)
    //            NowSpeed = MaxMoveSpeed * RunSpeedRate;
    //    }
    //    else
    //    {
    //        NowSpeed += MoveSpeedAcc * Time.deltaTime;
    //        if (NowSpeed > MaxMoveSpeed)
    //            NowSpeed = MaxMoveSpeed;
    //    }
    //    newDir *= NowSpeed;

    //    NowMoveVelocity = Vector3.Lerp(NowMoveVelocity, newDir, MoveVelocity_RotLerp).normalized;

    //    //NowMoveVelocity = Vector3.Lerp(NowMoveVelocity, newDir * _speed, MoveVelocity_RotLerp);
    //    //charaController.Move(NowMoveVelocity * Time.deltaTime);
    //}

    //void AddToVelocity(Vector3 newVelocity)
    //{

    //    NowMoveVelocity = Vector3.Lerp(NowMoveVelocity, newVelocity, MoveVelocity_RotLerp).normalized;

    //    float speed = NowMoveVelocity.magnitude;
    //    //  speed += _speed;
    //    NowMoveVelocity *= speed;
    //}

    public void SetPosition(Vector3 velocity)
    {
        charaController.Move(velocity);
    }
}
