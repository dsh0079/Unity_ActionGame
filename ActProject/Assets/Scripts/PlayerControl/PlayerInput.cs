using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum PInput
{
    Move_Front,
    Move_Back,
    Move_Left,
    Move_Right,
    Rush,
    Jump,
    Run,
    Roll,
    Acttak01,
    Acttak02,
    Acttak03,
}


/// <summary>
/// must on the camera
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { private set; get; }

    List<PInput> inputOnOneFrame = new List<PInput>();

    /// <summary>
    /// on X,Z plane
    /// </summary>
    public Vector3 InputDirecton = Vector3.zero;

    public void Init(PlayerInput p)
    {
        Instance = p;
    }

    public void Update()
    {
        if (Instance == null)
            return;
        inputOnOneFrame.Clear();

        InputDirectonUpdate();

        if (Input.GetKey(KeyCode.K))
        {
            inputOnOneFrame.Add(PInput.Run);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputOnOneFrame.Add(PInput.Jump);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            inputOnOneFrame.Add(PInput.Roll);
        }
    }

    private void InputDirectonUpdate()
    {
        InputDirecton = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            InputDirecton += this.transform.forward;
            inputOnOneFrame.Add(PInput.Move_Front);
        }
        if (Input.GetKey(KeyCode.S))
        {
            InputDirecton += -this.transform.forward;
            inputOnOneFrame.Add(PInput.Move_Back);
        }
        if (Input.GetKey(KeyCode.A))
        {
            InputDirecton += -this.transform.right;
            inputOnOneFrame.Add(PInput.Move_Left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            InputDirecton += this.transform.right;
            inputOnOneFrame.Add(PInput.Move_Right);
        }

        if(InputDirecton != Vector3.zero)
        {
            InputDirecton.y = 0;
            InputDirecton = InputDirecton.normalized;
        }
    }

    public bool IsOn(PInput input)
    {
        if (inputOnOneFrame.Contains(input) == true)
            return true;

        return false;
    }

    public bool IsAllOn(params PInput[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (IsOn(inputs[i]) == false)
                return false;
        }

        return true;
    }

}
