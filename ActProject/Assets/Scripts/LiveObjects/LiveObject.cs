using UnityEngine;
using System.Collections;

public delegate void OnDeadDel(LiveObject lo);

public class LiveObject : MonoBehaviour
{
    public int ID { private set; get; }
    public bool IsDead { private set; get; }
    public int HP { protected set; get; }

    public float DestroyDelay = 3f;
    public OnDeadDel OnDeadEvent;

    public virtual  void Init(int ID, int HP,Vector3 pos)
    {
        this.ID = ID;
        this.HP = HP;
        this.transform.position = pos;
        this.gameObject.SetActive(true);
    }

    public virtual void LUpdate()
    {
        if(IsDead == false)
        {
            if (HP <= 0)
            {
                IsDead = true;
                OnDead();
                GameObject.Destroy(this.gameObject, DestroyDelay);
            }
        }
    }

    public virtual void OnDead()
    {
        if (OnDeadEvent != null)
            OnDeadEvent(this);
    }

    public virtual void OnLDestroy()
    {

    }

    void Awake()
    {
        this.gameObject.SetActive(false);
    }

    void Start()
    {
    }

    void Update()
    {
        LUpdate();
    }

    void OnDestroy()
    {
        OnLDestroy();
    }
}
