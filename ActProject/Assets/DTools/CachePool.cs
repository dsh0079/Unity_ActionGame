using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 通用缓冲池
/// </summary>
public class CachePool 
{
    Dictionary<int, ICachePool> lives = new Dictionary<int, ICachePool>();
    List<ICachePool> idles = new List<ICachePool>();

    public delegate ICachePool GetNewDel();
    GetNewDel getNew;

    public CachePool(GetNewDel getNew)
    {
        this.getNew = getNew;
    }

    public bool ContainsKey(int id)
    {
        return lives.ContainsKey(id);
    }

    public ICachePool Get(int id)
    {
        return lives[id];
    }

    public void Release(int id)
    {
        if(lives.ContainsKey(id))
        {
            ICachePool icp = lives[id];
            icp.Release();
            lives.Remove(id);
            idles.Add(icp);
        }
    }

    public ICachePool GetNewOne(int newID)
    {
        ICachePool lh = null;
        if (idles.Count > 0)
        {
            lh = idles[0];
            idles.RemoveAt(0);
        }
        else
        {
            lh = getNew();
        }

        lh.Restart();
        lh.ID = newID;
        lives.Add(newID, lh);
        return lh;
    }

    public void ClearAll()
    {
        for (int i = 0; i < idles.Count; i++)
        {
            if (idles[i] != null)
                idles[i].Destroy();
        }
        idles.Clear();

        foreach (var item in lives)
        {
            if (item.Value != null)
                item.Value.Destroy();
        }
        lives.Clear();
    }
}

public interface ICachePool
{
    int ID { get; set; }
    //重新启用
    void Restart();
    //变为空闲的
    void Release();
    //永久性销毁
    void Destroy();

}
