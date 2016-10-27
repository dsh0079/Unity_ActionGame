using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RV_TEST : MonoBehaviour {

    int count;

    AAStruct AAStruct = new AAStruct();
    public GUIStyle guiStyle;

    public int public_AAA = 100;
    int private_BBB = 22;
    protected float protected_CCC = 122.5121f;
    float _float = 0.7153f;

    public string propertyS { get; set; }
    public int propertyA { get; set; }
    public int propertyC { get { return private_BBB; } }
    public int propertyCS { set { private_BBB = value; } }

    public string public_AAAstr = "sdfsdfg";
    string private_BBBstr = "sdgdg";
    protected string protected_CCCstr = "2342355";
    string private_DDDstr = null;

    Vector3 v3;
    Vector2 v2;
    Vector4 v4;
    Quaternion qua;

    EnumXX enumXX;

    List<string> list_str = new List<string>() { "12312", "asdasd" ,null};
    List<AAStruct> list_Struct = new List<AAStruct>() { new AAStruct(),new AAStruct()};
    List<Classsa> list_Classs = new List<Classsa>() { new Classsa(), new Classsa(), null};
    Dictionary<string, int> dic = new Dictionary<string, int>();

    RV_TEST rv_TEST;


    int i = 99;
    int ASDASFSDSFSDGFSDGGO = 99;

    // Use this for initialization
    void Start () {
        count = 0;

        dic.Add("sdfsdf", 213);
        dic.Add("sd32fsdf", 213);
        dic.Add("sd3fsdf", 213);
    }
	
	// Update is called once per frame
	void Update () {

        count++;

    }
}


public struct AAStruct
{
    public int haha;
}

public class Classsa
{
    public int haha;
}

public enum EnumXX
{
   EXX,
   EYY
}
