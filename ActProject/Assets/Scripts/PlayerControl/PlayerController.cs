using UnityEngine;
using System.Collections;
using DTool;

public class PlayerController : MonoBehaviour {

    PlayerCharater chara;
    ActionMachine actionMachine; //只是玩家使用，AI将使用其他Machine

    public void Init()
    {
    //    charaController = this.GetComponent<CharacterController>();
        this.chara = this.GetComponent<PlayerCharater>();

        actionMachine = new ActionMachine(this.chara);
    }

    void Update()
    {

        if (actionMachine != null)
            actionMachine.AMUpdate();

        ShowMessage.Add("Player Act State", this.actionMachine.NowState.State);
    }

    void  FixedUpdate()
    {
        if (actionMachine != null)
            actionMachine.FixedUpdate();
    }
}
