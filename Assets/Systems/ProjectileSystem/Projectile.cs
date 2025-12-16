using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    Faction faction;
    public Faction FactionType=>faction;
    float damage;
    public float Damage=>damage;
    float speed = 3f;
    float elapsed = 0f;
    //float height = 5f;
    
    Vector3 targetPosition;
    Vector3 startPosition;

    Vector3 lastPosition;
    //float minHeight = 0.5f;
    //float maxHeight = 10f;

    Func<Vector3, Vector3, float, Vector3> lerpFunction;
    public event System.Action<Collider,Projectile> OnTriggerEnterCallBack;
    public void Init(Faction faction,Vector3 startPosition,Vector3 targetPosition, float damage, Func<Vector3, Vector3, float ,Vector3> lerpFunc, System.Action<Collider, Projectile> onTriggerEnterCallBack)
    {
        this.faction = faction;
        this.startPosition = startPosition;
        gameObject.transform.position = startPosition;
        this.targetPosition = targetPosition;
        this.elapsed = 0f;

        Vector3 flatStart = startPosition;
        Vector3 flatTarget = targetPosition;
        flatStart.y = 0f;
        flatTarget.y = 0f;
        float distance = Vector3.Distance(flatStart, flatTarget);
        //this.height = Mathf.Clamp(
        //     distance * distance * 0.02f,
        //     minHeight,
        //     maxHeight
        // );

        this.damage = damage;
        Activate();
        lerpFunction = lerpFunc;
        OnTriggerEnterCallBack = onTriggerEnterCallBack;
    }
    private void Update()
    {
        elapsed += Time.deltaTime * speed;
        elapsed = Mathf.Clamp01(elapsed);
        //float heightOffset = height * elapsed * (1 - elapsed);
        Vector3 position = lerpFunction(startPosition, targetPosition, elapsed);
        //position.y += heightOffset;
        Vector3 direction = (position - lastPosition).normalized;
        transform.up = direction;
        transform.position = position;
        lastPosition = position;
        if (elapsed >= 1f)
        {
            Deactivate();
        }
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        //if(!other.TryGetComponent<HealthComponent>(out var targetHealth))
        //{
        //    return;
        //}

        //if (targetHealth.Stats.FactionType != this.faction)
        //{
        //    targetHealth.DeductHealth(damage);
        //    Deactivate();
        //}
        OnTriggerEnterCallBack?.Invoke(other,this);
    }
}
