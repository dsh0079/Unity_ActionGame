using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RV_TEST3 : MonoBehaviour
{
    enum  EnumTest
    {
        Enum01,
        Enum02,
        Enum03,
    }

    string input_str_null = null;
    string input_str = "str";
    int input_int = 1;
    sbyte input_sbyte = 1;
    byte input_byte = 1;
    short input_short = 1;
    ushort input_ushort = 1;
    uint input_uint = 1;
    long input_long = 1;
    ulong input_ulong = 1;
    decimal input_decimal = 1;



    float input_float = 2;
    double input_double = 3;
    EnumTest input_Enum;
    bool input_bool;
    Vector2 input_Vector2;
    Vector3 input_Vector3;
    Vector4 input_Vector4;


    Color input_Color;
    Color32 input_Color32;

    RV_TEST3 input_class = null;
    RV_TEST3 input_class_null = null;

    ClassA classA_____________________________;

    List<int> int_List__________ = new List<int>();
    List<string> string_List = null;
    ArrayList arrayList = null;
    Dictionary<int, string> dic = null;
    float[] float_Array = null;

    // Use this for initialization
    void Start()
    {
        input_class = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

class ClassA
{
    int a = 0;
}