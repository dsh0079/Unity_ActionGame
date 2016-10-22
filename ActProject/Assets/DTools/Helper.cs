using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DTool
{
    /// <summary>
    /// 各种通用的快捷方法
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// 遍历所有子节点寻找指定目标上的指定脚本
        /// </summary> 
        public static T FindChild<T>(Transform parent, string childName) where T : Component
        {
            //find all child node
            Transform target = FindTargetFromChildNode(parent, childName);

            if (target != null)
            {
                T t = target.gameObject.GetComponent<T>();
                if (t == null)
                {
                    Debug.LogError(string.Format("Helper -> FindChild(...) find child name is '{0}'  in '{1}' , but not exist component '{2}' in it ... ", childName, parent.name,
                        typeof(T).ToString()));
                }
                return t;
            }
            else
            {
                Debug.LogError(string.Format("Helper -> FindChild(...) can't child name is '{0}'  in '{1}' ... ", childName, parent.name));
                return null;
            }
        }

        public static T FindChild<T>(GameObject parent, string childName) where T : Component
        {
            return FindChild<T>(parent.transform, childName);
        }

        /// <summary>
        /// 遍历所有子节点寻找指定目标
        /// </summary>
        public static GameObject FindChild(Transform parent, string childName)
        {
            //find all child node
            Transform target = FindTargetFromChildNode(parent, childName);

            if (target != null)
            {
                return target.gameObject;
            }
            else
            {
                Debug.LogError(string.Format("Helper -> FindChild(...) can't child name is '{0}'  in '{1}' ... ", childName, parent.name));
                return null;
            }
        }

        static void FindTargetFromChildNode(Transform parent, string childName, out Transform target)
        {
            target = parent.FindChild(childName);
            if (target != null)
                return;

            if (parent.childCount > 0)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform t = parent.GetChild(i);
                    FindTargetFromChildNode(t, childName, out target);
                }
            }
        }

        static Transform FindTargetFromChildNode(Transform parent, string childName)
        {
            Transform target = parent.FindChild(childName);
            if (target == null)
            {
                if (parent.childCount > 0)
                {
                    for (int i = 0; i < parent.childCount; i++)
                    {
                        Transform t = parent.GetChild(i);
                        target = FindTargetFromChildNode(t, childName);
                        if (target != null)
                            return target;
                    }
                }
            }
            else
            {
                return target;
            }
            return null;
        }

        /// <summary>
        /// 鼠标坐标转换到世界坐标
        /// </summary>
        public static Vector3 GetMouseWorldPos(Plane plane)
        {
            //鼠标在屏幕上的位置.
            Vector3 cursorScreenPosition = Input.mousePosition;
            //在鼠标所在的屏幕位置发出一条射线(暂名该射线为x射线).
            Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);
            //定义一个float变量(该变量会在下面的代码里用到).
            float dist;
            //目标平面与x射线相交(这句代码的作用是得到x射线与目标平面相交时,x射线射出多远---即得到dist的值).
            plane.Raycast(ray, out dist);
            //在x射线这条射线上,距离起点dist处的点(该点即为鼠标在世界空间里的坐标).
            return ray.GetPoint(dist);
        }

        /// <summary>
        ///屏幕坐标转换到世界坐标
        /// </summary>
        public static Vector3 GetScreenPointWorldPos(Plane plane, Vector3 screenPoint)
        {
            //鼠标在屏幕上的位置.
            Vector3 cursorScreenPosition = screenPoint;
            //在鼠标所在的屏幕位置发出一条射线(暂名该射线为x射线).
            Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);
            //定义一个float变量(该变量会在下面的代码里用到).
            float dist;
            //目标平面与x射线相交(这句代码的作用是得到x射线与目标平面相交时,x射线射出多远---即得到dist的值).
            plane.Raycast(ray, out dist);
            //在x射线这条射线上,距离起点dist处的点(该点即为鼠标在世界空间里的坐标).
            return ray.GetPoint(dist);
        }

        /// <summary>
        /// 获取正交摄像机的区域的4个点,z始终是0, 左下开始,顺时针, XY平面
        /// </summary>
        public static Vector3[] GetOrthographicCameraCorners(Camera camera)
        {
            Vector3[] result = new Vector3[4];

            float size = camera.orthographicSize;
            float height_half = size;
            float width_half = ((float)camera.pixelWidth / (float)camera.pixelHeight) * height_half;

            //Canvas.srceen
            result[0] = camera.transform.position + new Vector3(-width_half, -height_half, 0);
            result[1] = camera.transform.position + new Vector3(-width_half, height_half, 0);
            result[2] = camera.transform.position + new Vector3(width_half, height_half, 0);
            result[3] = camera.transform.position + new Vector3(width_half, -height_half, 0);

            return result;
        }

        /// <summary>
        /// 获取透视摄像机视口的4个坐标,顺序是
        /// UpperLeft , UpperRight , LowerLeft , LowerRight
        /// </summary>
        public static Vector3[] GetPerspectiveCameraCorners(float distance, Camera theCamera)
        {
            Vector3[] corners = new Vector3[4];

            float halfFOV = (theCamera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
            float aspect = theCamera.aspect;

            float height = distance * Mathf.Tan(halfFOV);
            float width = height * aspect;

            // UpperLeft
            corners[0] = theCamera.transform.position - (theCamera.transform.right * width);
            corners[0] += theCamera.transform.up * height;
            corners[0] += theCamera.transform.forward * distance;

            // UpperRight
            corners[1] = theCamera.transform.position + (theCamera.transform.right * width);
            corners[1] += theCamera.transform.up * height;
            corners[1] += theCamera.transform.forward * distance;

            // LowerLeft
            corners[2] = theCamera.transform.position - (theCamera.transform.right * width);
            corners[2] -= theCamera.transform.up * height;
            corners[2] += theCamera.transform.forward * distance;

            // LowerRight
            corners[3] = theCamera.transform.position + (theCamera.transform.right * width);
            corners[3] -= theCamera.transform.up * height;
            corners[3] += theCamera.transform.forward * distance;

            return corners;
        }

        /// <summary>
        /// 判断指定的位置在不在屏幕中, XY平面
        /// </summary>
        public static bool IsInScreen(Camera camera, Vector3 pos)
        {
            Vector3[] v3 = GetOrthographicCameraCorners(camera);

            if (pos.y < v3[0].y || pos.y > v3[1].y ||
                pos.x < v3[0].x || pos.x > v3[3].x)
            {
                return false;
            }
            //if (pos.y > v3[0].y && pos.y < v3[1].y &&
            //    pos.x > v3[0].x && pos.x < v3[3].x)
            //{
            //    return true;
            //}
            //return false;
            return true;
        }

        /// <summary>
        /// 把一个坐标限制在屏幕内, 返回超出屏幕的方向 GetOrthographicCameraCorners
        /// </summary>
        public static Vector3 LimitPosInTheScreen(Camera camera, Vector3 pos, float offset, out Vector2 outDir)
        {
            Vector3[] v3 = GetOrthographicCameraCorners(camera);
            outDir = Vector2.zero;

            if (pos.x < v3[0].x + offset)
            {
                pos.x = v3[0].x + offset;
                outDir = new Vector2(-1, 0);
            }
            if (pos.x > v3[3].x - offset)
            {
                pos.x = v3[3].x - offset;
                outDir = new Vector2(1, 0);
            }

            if (pos.y < v3[0].y + offset)
            {
                pos.y = v3[0].y + offset;
                outDir = new Vector2(0, -1);
            }
            if (pos.y > v3[1].y - offset)
            {
                pos.y = v3[1].y - offset;
                outDir = new Vector2(0, 1);
            }

            return pos;
        }

        /// <summary>
        /// 把一个坐标限制在屏幕内,正交摄像机
        /// </summary>
        public static Vector3 LimitPosInTheScreen(Camera camera, Vector3 pos, float offset)
        {
            Vector3[] v3 = GetOrthographicCameraCorners(camera);

            if (pos.x < v3[0].x + offset)
                pos.x = v3[0].x + offset;
            if (pos.x > v3[3].x - offset)
                pos.x = v3[3].x - offset;

            if (pos.y < v3[0].y + offset)
                pos.y = v3[0].y + offset;
            if (pos.y > v3[1].y - offset)
                pos.y = v3[1].y - offset;

            return pos;
        }

        /// <summary>
        /// 随机方向, XY平面
        /// </summary>
        public static Vector2 RandomVector2()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        /// <summary>
        /// 随机方向 
        /// </summary>
        public static Vector3 RandomVector3()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        /// <summary>
        /// 随机方向, XY平面
        /// </summary>
        public static Vector3 RandomVector3XY()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        }

        public static Vector3 RandomVector3XY(float min, float max)
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f) * Random.Range(min, max);
        }

        /// <summary>
        /// 获得所有第一层的子物体
        /// </summary>
        public static List<GameObject> GetAllDirectChildren(GameObject parent)
        {
            List<GameObject> children = new List<GameObject>();

            if (parent == null)
                return children;

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                children.Add(parent.transform.GetChild(i).gameObject);
            }

            return children;
        }

        /// <summary>
        /// 找到parent及其所有子物体上的指定组件
        /// </summary>
        public static List<T> GetAllComponents<T>(GameObject parent) where T : Component
        {
            List<T> result = new List<T>();

            GetAllComponents<T>(parent, ref result);
            return result;
        }
        static void GetAllComponents<T>(GameObject parent, ref List<T> result) where T : Component
        {
            if (parent == null)
                return;
            T t = parent.GetComponent<T>();
            if (t != null)
                result.Add(t);

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GetAllComponents(parent.transform.GetChild(i).gameObject, ref result);
            }
        }

        /// <summary>
        /// 转换时间格式,从XXs到XX:XX
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string ConvertTime(float second)
        {
            System.TimeSpan a = System.TimeSpan.FromMilliseconds(second * 1000);
            //PadLeft(2, '0') 一共2位,位数不够时从左边开始用0补
            string str = a.Minutes.ToString().PadLeft(1, '0') + ":" +
                 a.Seconds.ToString().PadLeft(2, '0') + ":" +
                 (a.Milliseconds / 10).ToString().PadLeft(2, '0');

            return str;
        }

        /// <summary>
        /// 找到parent及其所有子物体上的指定组件
        /// </summary>
        public static List<GameObject> GetAllChildren(GameObject parent, string name)
        {
            List<GameObject> result = new List<GameObject>();

            GetAllChildren(parent, name, ref result);
            return result;
        }
        static void GetAllChildren(GameObject parent, string name, ref List<GameObject> result)
        {
            if (parent == null)
                return;
            if (parent.name == name)
                result.Add(parent);

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GetAllChildren(parent.transform.GetChild(i).gameObject, name, ref result);
            }
        }

        /// <summary>
        /// 返回0~1之间的数
        /// </summary>
        /// <returns></returns>
        public static float Prob()
        {
            return Random.Range(0, 1f);
        } 

        /// <summary>
        /// 序列化 对象到字符串
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <returns>序列化后的字符串</returns>
        public static string Serialize<T>(T obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Close();
                return System.Convert.ToBase64String(buffer);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("序列化失败,原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 反序列化 字符串到对象
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <param name="str">要转换为对象的字符串</param>
        /// <returns>反序列化出来的对象</returns>
        public static T Desrialize<T>(T obj, string str)
        {
            try
            {
                obj = default(T);
                IFormatter formatter = new BinaryFormatter();
                byte[] buffer = System.Convert.FromBase64String(str);
                MemoryStream stream = new MemoryStream(buffer);
                obj = (T)formatter.Deserialize(stream);
                stream.Flush();
                stream.Close();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("反序列化失败,原因:" + ex.Message);
            }
            return obj;
        }

        public static T RaycastFirst<T>(Vector3 pos, Vector3 dir, float maxDistance) where T: Component
        {
            Ray ray = new Ray(pos, dir);
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);

            for (int i = 0; i < hits.Length; i++)
            {
                T b = hits[i].collider.GetComponent<T>();
                if (b != null)
                    return b;
            }

            return null;
        }

        public static List<T> RaycastAll<T>(Vector3 pos, Vector3 dir, float maxDistance) where T : Component
        {
            List<T> result = new List<T>();
            Ray ray = new Ray(pos, dir);
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);

            for (int i = 0; i < hits.Length; i++)
            {
                T b = hits[i].collider.GetComponent<T>();
                if (b != null)
                    result.Add(b);
            }

            return result;
        }

        public static T SphereCastFirst<T>(Vector3 pos, Vector3 dir, float  radius) where T : Component
        {
           // Ray ray = new Ray(pos, dir);
            RaycastHit[] hits = Physics.CapsuleCastAll(pos, pos, radius, dir,0f);
          
            for (int i = 0; i < hits.Length; i++)
            {
                T b = hits[i].collider.GetComponent<T>();
                if (b != null)
                    return b;
            }

            return null;
        }

        public static List<GameObject> SphereCast(Vector3 pos, Vector3 dir, float radius)  
        {
            List<GameObject> result = new List<GameObject>();
        //    Ray ray = new Ray(pos, dir);
            RaycastHit[] hits = Physics.CapsuleCastAll(pos, pos, radius, dir, 0f);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != null && hits[i].collider.gameObject != null)
                {
                    result.Add(hits[i].collider.gameObject);
                }
            }

            return result;
        }

        public static List<T> SphereCast<T>(Vector3 pos, Vector3 dir, float radius) where T : Component
        {
            List<T> result = new List<T>();
          //  Ray ray = new Ray(pos, dir);
            RaycastHit[] hits = Physics.CapsuleCastAll(pos, pos, radius, dir, 0f);

            for (int i = 0; i < hits.Length; i++)
            {
                T b = hits[i].collider.GetComponent<T>();
                if (b != null)
                {
                    result.Add(b);
                }
            }

            return result;
        }

        public static bool IsInRange(Vector3 rangeCenter, float rangeRadius, Vector3 target)
        {
            if ((target - rangeCenter).sqrMagnitude <= rangeRadius * rangeRadius)
                return true;
            return false;
        }

        public static Vector3 GetRayHitPoint(Vector3 shootPos, Vector3 dir, float maxDis)
        {
            Ray ray = new Ray(shootPos, dir);
            RaycastHit mHit;
            if (Physics.Raycast(ray, out mHit, maxDis))
            {
                return mHit.point;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// 距离是否小于给定的值
        /// </summary>
        public static bool IsDistanceLess(Vector3 v1,Vector3 v2,float dis)
        {
            if ((v1 - v2).sqrMagnitude < dis * dis)
                return true;
            return false;
        }
    }
}