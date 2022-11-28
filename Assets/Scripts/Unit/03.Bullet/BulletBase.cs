using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBase : UnitBase
{
    public float LifeTime { get; set; } = 3f;
    public float Damage { get; set; } = 1f;
    private float _lifeTime = 3f;

    protected override void Init()
    {
        base.Init();
        _lifeTime = LifeTime;
    }

    protected override void Awake()
    {
        base.Init();
        var move = AddBehaviour<BulletMove>();
        move.Speed = 10;
        
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        Alive();
    }
    
    private void Alive()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0)
        {
            LifeTime = _lifeTime;
            GameManager.Instance.GetManager<PoolManager>().EnqueueObject(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(gameObject.tag))
            return;
        var theUnit = other.GetComponent<UnitBase>();
        if (theUnit == null) return;
        theUnit.State.Damage(Damage);
        LifeTime = _lifeTime;
        GameManager.Instance.GetManager<PoolManager>().EnqueueObject(gameObject);
    }
    
    public void SetBulletDir(Vector3 dir)
    {
        GetBehaviour<BulletMove>().Dir = dir;
    }
    
    public void SetBulletTarget(GameObject target)
    {
        GetBehaviour<BulletMove>().Target = target;
    }
}
