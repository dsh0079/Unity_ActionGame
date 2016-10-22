using UnityEngine;
using System.Collections;

/// <summary>
/// 倒计时 循环执行
/// </summary>
public class Countdown
{
    NoParameterDel method;
    //倒计时中一直执行
    NoParameterDel updateMethod;
    float maxTime;
    float nowTime = -1f;

    /// <summary>
    /// 是否倒计时结束,只有isOnce == true时才有意义,否则都是false
    /// </summary>
    public bool IsOver { private set; get; }
    //是否是一次性的
    bool isOnce = true;

    public Countdown(float maxTime, NoParameterDel method, NoParameterDel updateMethod)
    {
        this.maxTime = maxTime;
        this.method = method;
        this.updateMethod = updateMethod;
        IsOver = true;
    }

    public Countdown(float maxTime, NoParameterDel method)
    {
        this.maxTime = maxTime;
        this.method = method;
        IsOver = true;
    }

    public Countdown(float maxTime)
    {
        this.maxTime = maxTime;
        IsOver = true;
    }

    public void Update()
    {
        if (nowTime > 0f)
        {
            nowTime -= Time.deltaTime;
            if (updateMethod != null)
                updateMethod();
            if (nowTime <= 0f)
            {
                if (this.isOnce == false)
                    nowTime = maxTime;
                IsOver = true;
                if (method != null)
                    method();
            }
        }
    }

    public void Start(bool isOnce)
    {
        this.isOnce = isOnce;
        nowTime = maxTime;
        IsOver = false;
    }

    public void Start(float newMaxTime, bool isOnce)
    {
        this.isOnce = isOnce;
        maxTime = newMaxTime;
        nowTime = maxTime;
        IsOver = false;
    }

    public void StartImmediate(bool isOnce)
    {
        if (method != null)
            method();
        this.isOnce = isOnce;
        nowTime = maxTime;
        IsOver = false;
    }

    public void Stop()
    {
        nowTime = -1f;
    }
}
