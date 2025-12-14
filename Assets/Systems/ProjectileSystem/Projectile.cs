using UnityEngine;
using System.Collections.Generic;
public class Projectile : MonoBehaviour
{
    Faction faction;
    float damage;
    float duration = 1f;
    float elapsed = 0f;
    float height = 5f;
    
    Vector3 targetPosition;
    Vector3 startPosition;

    public void Init(Faction faction,Vector3 startPosition,Vector3 targetPosition, float damage, float duration=1f,float height =5f)
    {
        this.faction = faction;
        this.startPosition = startPosition;
        gameObject.transform.position = startPosition;
        this.targetPosition = targetPosition;
        this.duration = Mathf.Max(duration,1);
        this.elapsed = 0f;
        this.height = height;
        Activate();
    }
    private void Update()
    {
        elapsed += Time.deltaTime/duration;
        elapsed = Mathf.Clamp01(elapsed);
        float heightOffset = height * 4 * elapsed * (1 - elapsed);
        Vector3 position = Vector3.Lerp(startPosition, targetPosition, elapsed);
        position.y += heightOffset;
        transform.position = position;
        transform.up = CalTagent(elapsed).normalized;
        if(elapsed >= 1f)
        {
            Deactivate();
        }
    }

    Vector3 CalTagent(float t)
    {
        float dxdt = 1f;
        float dydt = height * 4 * (1 -2*elapsed);
        float dzdt = 0;

        Vector3 tangent = new Vector3 (dxdt, dydt, dzdt);
        return tangent;
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
        if(!other.TryGetComponent<HealthComponent>(out var targetHealth))
        {
            return;
        }

        if (targetHealth.Stats.FactionType != this.faction)
        {
            targetHealth.DeductHealth(damage);
            Deactivate();
        }
    }
}
