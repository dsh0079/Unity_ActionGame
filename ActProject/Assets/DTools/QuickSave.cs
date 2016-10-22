//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using System;

//namespace DTool
//{
//    /// <summary>
//    /// 保存数据到硬盘
//    /// 序列化类的形式
//    /// </summary>
//    public class QuickSave
//    {
//        static string fileName = "QuickSave.data";
//        static string path;
//        static DataCollection dc = null;

//        public static void Init(string _path)
//        {
//            path = _path + "/" + fileName;
//            Load();
//        }

//        #region  ...
//        public static void SaveToDisk()
//        {
//            if (dc == null)
//            { Debug.LogError("QuickSave -> SaveToDisk() error : 'DataCollection is null  ', need call 'Init()' at first!"); return; }
//            Save();
//        }

//        static  void Save()
//        {
//            Stream stre = null;
//            try
//            {
//                if (dc.IsEmpty() == true)
//                    return;
//                IFormatter ifor = new BinaryFormatter();
//                stre = new FileStream(path, FileMode.Create);
//                ifor.Serialize(stre, dc);
//                stre.Close();
//            }
//            catch (Exception ex)
//            {
//                if (stre != null)
//                    stre.Close();
//                Debug.LogError("QuickSave -> Save() error :" + ex.Message);
//            }
//        }

//        static void Load()
//        {
//            Stream stre = null;
//            try
//            {
//                IFormatter ifor = new BinaryFormatter();
//                stre = new FileStream(path, FileMode.Open, FileAccess.Read);
//                if (stre.Length != 0)
//                    dc = (DataCollection)ifor.Deserialize(stre);
//                else
//                    dc = new DataCollection();

//                stre.Close();
//            }
//            catch (Exception ex)
//            {
//                if (stre != null)
//                    stre.Close();
//                Debug.LogError("QuickSave -> Load() error :" + ex.Message);
//            }
//        }

//        static bool CheckName(string name)
//        {
//            if (name.Trim() == "" || name == null)
//            {
//                Debug.LogError("QuickSave -> CheckName() error : name can not be '' or null ........");
//                return false;
//            }
//            return true;
//        }

//        public static bool IsHadData(string dataName)
//        {
//            if (dc.Datas.ContainsKey(dataName))
//                return true;
//            return false;
//        }

//        public static bool IsHadDataList(string dataName)
//        {
//            if (dc.ListDatas.ContainsKey(dataName))
//                return true;
//            return false;
//        }

//        //public static void ClearAll()
//        //{
//        //    dc.Datas.Clear();
//        //    dc.ListDatas.Clear();
//        //    dc = null;
//        //}

//        #endregion

//        #region set ....

//        public static void SetInt(string name, int _value)
//        {
//            if (CheckName(name) == false)
//                return;

//            dc.Datas[name] = _value;
//        }

//        public static void SetFloat(string name, float _value)
//        {
//            if (CheckName(name) == false)
//                return;
//            dc.Datas[name] = _value;
//        }

//        public static void SetVector2(string name, Vector2 _value)
//        {
//            if (CheckName(name) == false)
//                return;
//            dc.Datas[name] = _value.x + "," + _value.y;
//        }

//        public static void SetVector2List(string name, List<Vector2> _values)
//        {
//            if (CheckName(name) == false)
//                return;

//            List<object> objs = new List<object>();
//            foreach (Vector2 v2 in _values)
//                objs.Add(v2.x + "," + v2.y);

//            dc.ListDatas[name] = objs;
//        }

//        //public void SetPoint2DList(string name, List<Point2D> _values)
//        //{
//        //    if (CheckName(name) == false)
//        //        return;

//        //    List<object> objs = new List<object>();
//        //    foreach (Point2D v2 in _values)
//        //        objs.Add(v2.X + "," + v2.Y);

//        //    dc.ListDatas[name] = objs;
//        //}

//        #endregion

//        #region  get ...
//        public static int GetInt(string name)
//        {
//            if (CheckName(name) == false)
//                return 0;
//            return Convert.ToInt32(dc.Datas[name]);
//        }

//        public static float GetFloat(string name)
//        {
//            if (CheckName(name) == false)
//                return 0;
//            return Convert.ToSingle(dc.Datas[name]);
//        }

//        public static List<Vector2> GetVector2List(string name)
//        {
//            List<Vector2> reuslt = new List<Vector2>();

//            if (CheckName(name) == false)
//                return reuslt;
//            try
//            {
//                List<object> objs = dc.ListDatas[name];
//                foreach (object v2 in objs)
//                {
//                    string[] strs = v2.ToString().Split(',');
//                    reuslt.Add(new Vector2(Convert.ToSingle(strs[0]), Convert.ToSingle(strs[1])));
//                }
//                return reuslt;
//            }
//            catch (System.Exception ex)
//            {
//                Debug.LogError("QuickSave -> GetVector2() error : " + ex.Message);
//                return null;
//            }
//        }

//        //public List<Point2D> GetPoint2DList(string name)
//        //{
//        //    List<Point2D> reuslt = new List<Point2D>();

//        //    if (CheckName(name) == false)
//        //        return reuslt;
//        //    try
//        //    {
//        //        List<object> objs = dc.ListDatas[name];
//        //        foreach (object v2 in objs)
//        //        {
//        //            string[] strs = v2.ToString().Split(',');
//        //            reuslt.Add(new Point2D(Convert.ToInt32(strs[0]), Convert.ToInt32(strs[1])));
//        //        }
//        //        return reuslt;
//        //    }
//        //    catch (System.Exception ex)
//        //    {
//        //        Debug.LogError("QuickSave -> GetPoint2DList() error : " + ex.Message);
//        //        return null;
//        //    }
//        //}

//        #endregion

//        [Serializable]
//        class DataCollection
//        {
//            public Dictionary<string, List<object>> ListDatas = new Dictionary<string, List<object>>();
//            public Dictionary<string, object> Datas = new Dictionary<string, object>();

//            public bool IsEmpty()
//            {
//                if (ListDatas.Count == 0 && Datas.Count == 0)
//                    return true;
//                return false;
//            }
//        }
//    }
//}