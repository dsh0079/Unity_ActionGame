using UnityEngine;
using System.Collections;
using DTool;

public class PlayerController : MonoBehaviour {

    PlayerCharater chara;
    //Vector3 nowMoveVelocity = Vector3.zero;
    //public float moveVelocity_RotLerp = 0.5f;
 //   CharacterController charaController;
    ActionMachine actionMachine; //其实应该在CharaterBase里,嘛,目前先不考虑AI 的情况

    public void Init()
    {
    //    charaController = this.GetComponent<CharacterController>();
        this.chara = this.GetComponent<PlayerCharater>();

        actionMachine = new ActionMachine(this.chara);
    }

    void Update()
    {
        //if (PlayerInput.Instance.IsOn(PInput.Jump))
        //{

        //}

        //if (PlayerInput.Instance.InputDirecton != Vector3.one)
        //{
        //    float speed = chara.MoveSpeed;

        //    if (PlayerInput.Instance.IsOn(PInput.Run))
        //        speed = chara.RunSpeed;

        //    chara.Move(PlayerInput.Instance.InputDirecton, speed);

        //    //nowMoveVelocity = Vector3.Lerp(nowMoveVelocity, PlayerInput.Instance.InputDirecton
        //    //    * speed, moveVelocity_RotLerp);

        //}

        //if (nowMoveVelocity != Vector3.zero)
        //{
        //  //  chara.Move(nowMoveVelocity, speed);
        //    //charaController.Move(nowMoveVelocity * Time.deltaTime);
        //}

        if (actionMachine != null)
            actionMachine.AMUpdate();

        Test();
    }

    void  FixedUpdate()
    {
        if (actionMachine != null)
            actionMachine.FixedUpdate();
    }

    void Test()
    {
        VisualTest.AddArrow("InputDirecton", 0.001f, this.transform.position,
         this.transform.position + this.chara.NowMoveVelocity *0.3f, Color.green);

        ShowMessage.Add("Player Act State", this.actionMachine.NowState.State);
    }
}
