using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RV_TEST2 : MonoBehaviour
{

    int count = 0;
    List<int> list = new List<int>() { 999,999 };
    List<List<int>> list2 = new List<List<int>>() { new List<int>() {100,101 }, new List<int>() { 1,2 } };

    Dictionary<List<int>, int> dic = new Dictionary<List<int>, int>();
    Dictionary<Dictionary<int, int>, int> dic2 = new Dictionary<Dictionary<int, int>, int>();

    void Start()
    {
        dic.Add(new List<int>() { 0, 1, 2 },199);
        dic.Add(new List<int>() { 0, 1, 2 },299);
        dic.Add(new List<int>() { 0, 1, 2 },399);

        Dictionary<int, int> _dic = new Dictionary<int, int>();
        _dic.Add(66, 77);
        _dic.Add(166, 177);
        dic2.Add(_dic, 100);

    }

    // Update is called once per frame
    void Update()
    {
        count++;

        if (count > 1111111110)
            count = 0;

        if(list.Count < 20)
        {
            list.Add(Random.Range(0, 99));
        }
        else
        {
            list.Clear();
            list.Add(count);
        }

    }
}
