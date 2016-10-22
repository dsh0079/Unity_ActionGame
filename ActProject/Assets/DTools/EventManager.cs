using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 事件参数基类
/// </summary>
public class EventArgs
{
    public object Parameter;
    /// <summary>
    /// 空参数
    /// </summary>
    public static EventArgs Null = new EventArgs(); 
}
 

/// <summary>
/// 事件管理类
/// </summary>
public class EventManager
{
    //单例模式.
    public static readonly EventManager Instance = new EventManager();
    private EventManager() { }

    //事件委托.
    public delegate void EventDelegate<T>(T e) where T : EventArgs;

    //保存所有事件接收方法 
    readonly Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

    /// <summary>
    /// 添加一个事件接收方法.
    /// </summary>
    public void AddListener<T>(EventDelegate<T> listener) where T : EventArgs
    {
        Delegate d;
        if (_delegates.TryGetValue(typeof(T), out d))
        {
            _delegates[typeof(T)] = Delegate.Combine(d, listener);
        }
        else
        {
            _delegates[typeof(T)] = listener;
        }
    }

    /// <summary>
    /// 删除一个事件接受方法
    /// </summary>
    public void RemoveListener<T>(EventDelegate<T> listener) where T : EventArgs
    {
        Delegate d;
        if (_delegates.TryGetValue(typeof(T), out d))
        {
            Delegate currentDel = Delegate.Remove(d, listener);

            if (currentDel == null)
            {
                _delegates.Remove(typeof(T));
            }
            else
            {
                _delegates[typeof(T)] = currentDel;
            }
        }
    }

    /// <summary>
    /// 发送事件
    /// </summary>
    public void Send<T>(T e) where T : EventArgs
    {
        if (e == null)
        {
            throw new ArgumentNullException("e");
        }

        Delegate d;
        if (_delegates.TryGetValue(typeof(T), out d))
        {
            EventDelegate<T> callback = d as EventDelegate<T>;
            if (callback != null)
            {
                callback(e);
            }
        }
    }
}