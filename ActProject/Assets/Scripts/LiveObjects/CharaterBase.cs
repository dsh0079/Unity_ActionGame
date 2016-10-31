using UnityEngine;
using System.Collections;
using DTool;

public class CharaterBase : LiveObject
{
    public Vector3 Position { get { return this.transform.position; } }
}