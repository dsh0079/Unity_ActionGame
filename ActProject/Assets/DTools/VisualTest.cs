using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DTool
{
    /// <summary>
    /// 可视化测试
    /// 
    /// 1. 可以添加同名的, 删除时会删除全部同名的
    /// 2. 2015.8.20 替换foreach
    /// </summary>
    public class VisualTest
    {
        static List<VisualObject> objects = new List<VisualObject>();
        public static readonly float PointRadius = 0.2f;

        public static void Update()
        {
            //ShowMessage.Add("VisualTest.Objects", objects.Count);
            // Whether the end of the display
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].LiveTime >= 0f && objects[i].IsShowOnce == true)
                {
                    objects[i].Update();
                    if (objects[i].LiveTime <= 0f)
                    {
                        objects.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public static void OnDrawGizmos()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].OnDraw();
            }
          //  foreach (VisualObject vo in objects)
          //      vo.OnDraw();
        }

        public static void AddCircle(string name, float liveTime, Vector3 center, float radius, Color color)
        {
            Add(new DTool.Circle(name, liveTime, center, radius, color));
        }

        public static void AddCircle(float liveTime, Vector3 center, float radius, Color color)
        {
            Add(new DTool.Circle("", liveTime, center, radius, color));
        }

        public static void AddPoint(string name, float liveTime, Vector3 pos, float pointRadius, Color color)
        {
            Add(new DTool.Point(name, liveTime, pos, pointRadius, color));
        }

        public static void AddPoint(float liveTime, Vector3 pos, Color color)
        {
            Add(new DTool.Point("", liveTime, pos, PointRadius, color));
        }

        public static void AddLine(string name, float liveTime, Vector3 start, Vector3 end, Color color)
        {
            Add(new DTool.Line(name, liveTime, start, end, color));
        }

        public static void AddCurve(string name, float liveTime, Color color, params Vector3[] points)
        {
            Add(new DTool.Curve(name, liveTime, color, points));
        }

        public static void AddArrow(string name, float liveTime, Vector3 from, Vector3 to, Color color)
        {
            Add(new DTool.Arrow(name, liveTime, from, to, color));
        }

        public static void AddArrow(string name, float liveTime, Vector3 dir, Vector3 pos, float length, Color color)
        {
            Add(new DTool.Arrow(name, liveTime, pos, pos + (dir * length), color));
        }

        public static void Delete(string name)
        {
            if (name == "")
                Debug.LogWarning("VisualTest -> Delete('') : all nameless visual objects will be deleted ...");
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Name == name)
                {
                    objects.RemoveAt(i);
                    i--;
                }
            }
        }

        static void Add(VisualObject vo)
        {
            objects.Add(vo);
        }

        public static void ClearAll()
        {
            objects.Clear();
        }
    }

    class VisualObject
    {
        public List<Vector3> points = new List<Vector3>();
        public string Name { private set; get; } // "" is Nameless
        public float LiveTime { private set; get; }// Less than 0 is unlimited
        public Color Color { private set; get; }
        public float PointRadius { protected set; get; } // Use to Point
        public bool IsShowOnce { private set; get; } // make ture all object show once at least, even livetime is 0f 

        public VisualObject(string name, float liveTime, Color color)
        {
            this.Color = color;
            this.Name = name;
            this.LiveTime = liveTime;
            this.IsShowOnce = false;
        }

        public void Update()
        {
            if (this.LiveTime >= 0)
                this.LiveTime -= Time.deltaTime;
        }

        public virtual void OnDraw()
        {
            this.IsShowOnce = true;
        }
    }

    class Line : VisualObject
    {
        public Line(string name, float liveTime, Vector3 startPoint, Vector3 endPoint, Color color)
            : base(name, liveTime, color)
        {
            this.points.Add(startPoint);
            this.points.Add(endPoint);
        }

        public override void OnDraw()
        {
            Gizmos.color = this.Color;
            Gizmos.DrawLine(points[0], points[1]);
            base.OnDraw();
        }
    }

    class Point : VisualObject
    {
        public Point(string name, float liveTime, Vector3 point, float pointRadius, Color color)
            : base(name, liveTime, color)
        {
            this.PointRadius = pointRadius;
            this.points.Add(point);
        }

        public override void OnDraw()
        {
            Gizmos.color = this.Color;
            Gizmos.DrawSphere(points[0], PointRadius);
            base.OnDraw();
        }
    }

    class Curve : VisualObject
    {
        public Curve(string name, float liveTime, Color color, params  Vector3[] points)
            : base(name, liveTime, color)
        {
            for (int i = 0; i < points.Length; i++)
            {
                this.points.Add(points[i]);
                
            }

            //foreach (Vector3 p in points)
            //    this.points.Add(p);
        }

        public override void OnDraw()
        {
            iTween.DrawPath(points.ToArray());
            base.OnDraw();
        }
    }

    class Arrow : VisualObject
    {
        public Arrow(string name, float liveTime, Vector3 from, Vector3 to, Color color)
            : base(name, liveTime, color)
        {
            this.points.Add(from);
            this.points.Add(to);
        }

        public override void OnDraw()
        {
            Gizmos.color = this.Color;
            Vector3 from = points[0];
            Vector3 to = points[1];
            float radius = Vector3.Distance(from, to) * 0.05f;
            Gizmos.DrawWireSphere(to, radius);
            Gizmos.DrawLine(from, to);
            base.OnDraw();
        }
    }

    //圆形（XZ平面的）. 
    class Circle : VisualObject
    {
        public Vector3 Center;
        public float Radius;

        public Circle(string name, float liveTime, Vector3 center, float radius, Color color)
            : base(name, liveTime, color)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public override void OnDraw()
        {
            points.Clear();
            Vector3 dir = new Vector3(0, 1, 0);
            for (int i = 1; i < 9; i++)
            {
                points.Add(Center + ((Quaternion.Euler(0, 0, i * 45) * dir).normalized * Radius));
            }
            points.Add(points[0]);

            iTween.DrawPath(points.ToArray(), new Color(this.Color.r, this.Color.g, this.Color.b, 0.5f));

            base.OnDraw();
        }
    }
}