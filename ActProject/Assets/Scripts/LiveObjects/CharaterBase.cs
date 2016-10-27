using UnityEngine;
using System.Collections;

public class CharaterBase : LiveObject
{

    public float MoveSpeed = 10f;

    public override void Init(int ID, int HP, Vector3 pos)
    {
        base.Init(ID, HP, pos);
    }

    public override void LUpdate()
    {
        base.LUpdate();
    }
}
