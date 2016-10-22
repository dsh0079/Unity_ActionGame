using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DTool
{
    /// <summary>
    /// 自动计时器，作用是 延迟执行方法.
    /// 即调用一次，指定时间后执行.
    /// 使用方法，加在一个始终活动的GameObject上就可以了.
    /// </summary>
    public class AutoTimer
    {
        static List<ATMethod> methods = new List<ATMethod>();

        public static string[] GetAllName()
        {
            string[] names = new string[methods.Count];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = methods[i].MethodID;
            }

            return names;
        }

        public static void Update()
        {
            //执行和清除.
            for (int i = 0; i < methods.Count; i++)
            {
                methods[i].Update();
                if (i < methods.Count && methods[i].IsOver == true)// i < methods.Count  保证嵌套删除没有错
                {
                    methods.RemoveAt(i);
                    i--;
                }
            }
        }

        //添加一个要延迟执行的方法.
        public static void AddDelay(string methodID, float delayTime, NoParameterDel method)
        {
            if (isExistedID(methodID) == true)
            {//ID已经存在 
                Debug.LogError("AutoTimer -> AddDelay() error : The same methodID '" + methodID + "' already exists ...");
                return;
            }
            ATMethod a = new DelayATMethod(methodID, delayTime, method, null, null);
            methods.Add(a);
        }
        //添加一个要延迟执行的方法.
        public static void AddDelay(string methodID, float delayTime, OneParameterDel method, object parameter)
        {
            if (isExistedID(methodID) == true)
            {//ID已经存在 
                Debug.LogError("AutoTimer -> AddDelay() error : The same methodID '" + methodID + "' already exists ...");
                return;
            }
            ATMethod a = new DelayATMethod(methodID, delayTime, null, method, parameter);
            methods.Add(a);
        }
        public static void AddDelay(float delayTime, NoParameterDel method)
        {
            AddDelay(IDBuilder.GetID("AT"), delayTime, method);
        }
        public static void AddDelay(float delayTime, OneParameterDel method, object parameter)
        {
            AddDelay(IDBuilder.GetID("AT"), delayTime, method, parameter);
        }

        //添加一个要间隔执行的方法. 返回true就停止执行
        public static void AddInterval(string methodID, float intervalTime, ReturnBoolDel method)
        {
            if (isExistedID(methodID) == true)
            {//ID已经存在 
                Debug.LogError("AutoTimer -> AddInterval() error : The same methodID '" + methodID + "' already exists ...");
                return;
            }
            ATMethod a = new IntervalATMethod(methodID, intervalTime, method);
            methods.Add(a);
        }
        //添加一个要间隔执行的方法.
        public static void AddInterval(string methodID, float intervalTime, NoParameterDel method)
        {
            if (isExistedID(methodID) == true)
            {//ID已经存在 
                Debug.LogError("AutoTimer -> AddInterval() error : The same methodID '" + methodID + "' already exists ...");
                return;
            }
            ATMethod a = new IntervalATMethod(methodID, intervalTime, method, null, null);
            methods.Add(a);
        }
        //添加一个要间隔执行的方法.
        public static void AddInterval(string methodID, float intervalTime, OneParameterDel method, object parameter)
        {
            if (isExistedID(methodID) == true)
            {//ID已经存在 
                Debug.LogError("AutoTimer -> AddInterval() error : The same methodID '" + methodID + "' already exists ...");
                return;
            }
            ATMethod a = new IntervalATMethod(methodID, intervalTime, null, method, parameter);
            methods.Add(a);
        }
        public static void AddInterval(float intervalTime, NoParameterDel method)
        {
            AddInterval(IDBuilder.GetID("AT"), intervalTime, method);
        }
        public static void AddInterval(float intervalTime, OneParameterDel method, object parameter)
        {
            AddInterval(IDBuilder.GetID("AT"), intervalTime, method, parameter);
        }

        //添加一个要每帧执行的方法. 返回true就停止执行,time到也停止
        public static void AddUpdate(string methodID, float time, ReturnBoolDel method)
        {
            if (isExistedID(methodID) == true)
            {//ID已经存在 
                Debug.LogError("AutoTimer -> AddUpdate() error : The same methodID '" + methodID + "' already exists ...");
                return;
            }
            ATMethod a = new UpdateATMethod(methodID, time, method);
            methods.Add(a);
        }
        public static void AddUpdate(float time, ReturnBoolDel method)
        {
            ATMethod a = new UpdateATMethod(IDBuilder.GetID("UpAT"), time, method);
            methods.Add(a);
        }
        public static void AddUpdate(ReturnBoolDel method)
        {
            ATMethod a = new UpdateATMethod(IDBuilder.GetID("UpAT"), method);
            methods.Add(a);
        }
        public static void AddUpdate(string methodID, ReturnBoolDel method)
        {
            if (isExistedID(methodID) == true)
            {//ID已经存在 
                Debug.LogError("AutoTimer -> AddUpdate() error : The same methodID '" + methodID + "' already exists ...");
                return;
            }
            ATMethod a = new UpdateATMethod(methodID, method);
            methods.Add(a);
        }
        public static void AddUpdate(string methodID, float time, OneParameterDel method, object _value)
        {
            if (isExistedID(methodID) == true)
            {//ID已经存在 
                Debug.LogError("AutoTimer -> AddUpdate() error : The same methodID '" + methodID + "' already exists ...");
                return;
            }
            ATMethod a = new UpdateATMethod(methodID, time, null, method, _value);
            methods.Add(a);
        }


        //删除一个要延迟/间隔/更新执行的方法.
        public static void DeleteMethod(string methodID)
        {
            for (int i = 0; i < methods.Count; i++)
            {
                if (methods[i].MethodID == methodID)
                {
                    methods.RemoveAt(i);
                    break;
                }
            }
        }

        public static void ClearAll()
        {
            methods.Clear();
        }

        //ID是否已经存在.
        static bool isExistedID(string id)
        {
            foreach (ATMethod a in methods)
            {
                if (a.MethodID == id)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// AutoTimer的内部类，用于保存方法和时间等信息.
        /// </summary>
        class ATMethod
        {
            public bool IsOver { protected set; get; } //是否结束,并可以消耗了
            public readonly string MethodID;

            protected float time;
            readonly NoParameterDel noMethod;
            readonly OneParameterDel oneMethod;
            readonly object _value;

            public ATMethod(string methodID, float time, NoParameterDel noMethod, OneParameterDel oneMethod, object _value)
            {
                this.MethodID = methodID;
                this.noMethod = noMethod;
                this._value = _value;
                this.oneMethod = oneMethod;
                this.time = time;
                IsOver = false;
            }

            public virtual void Update()
            {
                if (time > 0)
                    time -= UnityEngine.Time.deltaTime;
            }

            protected void Invoke()
            {
                if (noMethod != null)
                    noMethod();
                if (oneMethod != null)
                    oneMethod(_value);
            }
        }

        /// <summary>
        /// 延迟执行的方法
        /// </summary>
        class DelayATMethod : ATMethod
        {
            public DelayATMethod(string methodID, float time, NoParameterDel noMethod, OneParameterDel oneMethod, object _value)
                : base(methodID, time, noMethod, oneMethod, _value)
            {
            }

            public override void Update()
            {
                if (this.IsOver == false && this.time <= 0)
                {
                    this.Invoke();
                    this.IsOver = true;
                }

                base.Update();
            }
        }

        /// <summary>
        /// 间隔执行的方法
        /// </summary>
        class IntervalATMethod : ATMethod
        {
            readonly float intervalTime;
            ReturnBoolDel boolMethod;

            public IntervalATMethod(string methodID, float _time, NoParameterDel noMethod, OneParameterDel oneMethod, object _value)
                : base(methodID, _time, noMethod, oneMethod, _value)
            {
                this.intervalTime = _time;
            }

            public IntervalATMethod(string methodID, float _time, ReturnBoolDel boolMethod)
                : base(methodID, _time, null, null, null)
            {
                this.boolMethod = boolMethod;
                this.intervalTime = _time;
            }

            public override void Update()
            {
                if (this.time <= 0)
                {
                    if (boolMethod != null)
                        this.IsOver = boolMethod();
                    this.Invoke();
                    this.time = this.intervalTime;
                }

                base.Update();
            }
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        class UpdateATMethod : ATMethod
        {
            ReturnBoolDel boolMethod;
            bool isInfiniteTime = false;

            public UpdateATMethod(string methodID, float time, NoParameterDel noMethod, OneParameterDel oneMethod, object _value)
                : base(methodID, time, noMethod, oneMethod, _value)
            {
            }

            public UpdateATMethod(string methodID, float time, ReturnBoolDel boolMethod)
                : base(methodID, time, null, null, null)
            {
                this.boolMethod = boolMethod;
            }

            public UpdateATMethod(string methodID, ReturnBoolDel boolMethod)
                : base(methodID, -1, null, null, null)
            {
                isInfiniteTime = true;
                this.boolMethod = boolMethod;
            }

            public override void Update()
            {
                if (isInfiniteTime == false && this.time <= 0)
                {
                    this.IsOver = true;
                    return;
                }
                else
                {
                    this.Invoke();
                    if (boolMethod != null)
                        this.IsOver = boolMethod();
                }
                base.Update();
            }
        }
    }
}