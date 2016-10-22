using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DTool
{
    /// <summary>
    /// 产生唯一ID
    /// 
    /// ID 都从1开始, 数字ID和字符串ID都不会又重复
    /// </summary>
    public class IDBuilder
    {
        static int digitalID = 0;
        static Dictionary<string, int> stringIDs = new Dictionary<string, int>();

        public static string GetID(string name)
        {
            name = name.Trim();
            if (name == "")
                Debug.LogError("IDBuilder -> GetID(...) error : can't gei id name is empty .....");

            if (stringIDs.ContainsKey(name) == false)
                stringIDs.Add(name, 0);

            stringIDs[name]++;
            return name + "_" + stringIDs[name];
        }

        public static int GetID()
        {
            digitalID++;
            return digitalID;
        }
    }
}