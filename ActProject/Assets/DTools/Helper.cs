using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DTool
{
    /// <summary>
    /// ����ͨ�õĿ�ݷ���
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// ���������ӽڵ�Ѱ��ָ��Ŀ���ϵ�ָ���ű�
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
        /// ���������ӽڵ�Ѱ��ָ��Ŀ��
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
        /// �������ת������������
        /// </summary>
        public static Vector3 GetMouseWorldPos(Plane plane)
        {
            //�������Ļ�ϵ�λ��.
            Vector3 cursorScreenPosition = Input.mousePosition;
            //��������ڵ���Ļλ�÷���һ������(����������Ϊx����).
            Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);
            //����һ��float����(�ñ�����������Ĵ������õ�).
            float dist;
            //Ŀ��ƽ����x�����ཻ(������������ǵõ�x������Ŀ��ƽ���ཻʱ,x���������Զ---���õ�dist��ֵ).
            plane.Raycast(ray, out dist);
            //��x��������������,�������dist���ĵ�(�õ㼴Ϊ���������ռ��������).
            return ray.GetPoint(dist);
        }

        /// <summary>
        ///��Ļ����ת������������
        /// </summary>
        public static Vector3 GetScreenPointWorldPos(Plane plane, Vector3 screenPoint)
        {
            //�������Ļ�ϵ�λ��.
            Vector3 cursorScreenPosition = screenPoint;
            //��������ڵ���Ļλ�÷���һ������(����������Ϊx����).
            Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);
            //����һ��float����(�ñ�����������Ĵ������õ�).
            float dist;
            //Ŀ��ƽ����x�����ཻ(������������ǵõ�x������Ŀ��ƽ���ཻʱ,x���������Զ---���õ�dist��ֵ).
            plane.Raycast(ray, out dist);
            //��x��������������,�������dist���ĵ�(�õ㼴Ϊ���������ռ��������).
            return ray.GetPoint(dist);
        }

        /// <summary>
        /// ��ȡ����������������4����,zʼ����0, ���¿�ʼ,˳ʱ��, XYƽ��
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
        /// ��ȡ͸��������ӿڵ�4������,˳����
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
        /// �ж�ָ����λ���ڲ�����Ļ��, XYƽ��
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
        /// ��һ��������������Ļ��, ���س�����Ļ�ķ��� GetOrthographicCameraCorners
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
        /// ��һ��������������Ļ��,���������
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
        /// �������, XYƽ��
        /// </summary>
        public static Vector2 RandomVector2()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        /// <summary>
        /// ������� 
        /// </summary>
        public static Vector3 RandomVector3()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        /// <summary>
        /// �������, XYƽ��
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
        /// ������е�һ���������
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
        /// �ҵ�parent���������������ϵ�ָ�����
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
        /// ת��ʱ���ʽ,��XXs��XX:XX
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string ConvertTime(float second)
        {
            System.TimeSpan a = System.TimeSpan.FromMilliseconds(second * 1000);
            //PadLeft(2, '0') һ��2λ,λ������ʱ����߿�ʼ��0��
            string str = a.Minutes.ToString().PadLeft(1, '0') + ":" +
                 a.Seconds.ToString().PadLeft(2, '0') + ":" +
                 (a.Milliseconds / 10).ToString().PadLeft(2, '0');

            return str;
        }

        /// <summary>
        /// �ҵ�parent���������������ϵ�ָ�����
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
        /// ����0~1֮�����
        /// </summary>
        /// <returns></returns>
        public static float Prob()
        {
            return Random.Range(0, 1f);
        } 

        /// <summary>
        /// ���л� �����ַ���
        /// </summary>
        /// <param name="obj">���Ͷ���</param>
        /// <returns>���л�����ַ���</returns>
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
                throw new System.Exception("���л�ʧ��,ԭ��:" + ex.Message);
            }
        }

        /// <summary>
        /// �����л� �ַ���������
        /// </summary>
        /// <param name="obj">���Ͷ���</param>
        /// <param name="str">Ҫת��Ϊ������ַ���</param>
        /// <returns>�����л������Ķ���</returns>
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
                throw new System.Exception("�����л�ʧ��,ԭ��:" + ex.Message);
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
        /// �����Ƿ�С�ڸ�����ֵ
        /// </summary>
        public static bool IsDistanceLess(Vector3 v1,Vector3 v2,float dis)
        {
            if ((v1 - v2).sqrMagnitude < dis * dis)
                return true;
            return false;
        }
    }
}