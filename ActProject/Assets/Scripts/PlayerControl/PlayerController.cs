using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    PlayerCharater chara;

    public void Init()
    {
        this.chara = this.GetComponent<PlayerCharater>();
    }

    void Update()
    {
        if (PlayerInput.Instance.InputDirecton != Vector3.one)
        {
            this.transform.position += PlayerInput.Instance.InputDirecton * chara.MoveSpeed * Time.deltaTime;
        }
    }
}
