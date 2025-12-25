using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StatsComponent))]
public class HealthComponent : MonoBehaviour
{
    public event Action OnDeath;
    private StatsComponent stats;
    public bool IsDead = false;
    public StatsComponent Stats
    {
        get
        {
            stats??= GetComponent<StatsComponent>();
            return stats;
        }
    }
    public float MaxHealth;
    public float CurrentHealth;

    [SerializeField] Image FillImage;

    private void Start()
    {
        Init(MaxHealth);
    }

    public void Init(float MaxHealth)
    {
        CurrentHealth = MaxHealth;
        this.MaxHealth = MaxHealth;
        GameManager.Instance.TickSystem.Subscribe(updateUI);
    }

    void updateUI()
    {
        //CurrentHealth -=Time.deltaTime;
        FillImage.fillAmount = CurrentHealth/MaxHealth;
        if (CurrentHealth<=0)
        {
            GameManager.Instance.TickSystem.Unsubscribe(updateUI);
            OnDeath?.Invoke();
            IsDead = true;
            //GlobalEffectManager.Instance.OnDeathEffect?.Invoke(transform.position);
            AnimationPool.Instance.Play_CFXR2_Skull_Head_Alt_AnimationAtFor(transform.forward,transform.position+(Vector3.up*0.5f),3f);
            Destroy(gameObject);
        }
    }

    public void DeductHealth(float amount)
    {
        amount = Mathf.Clamp(amount,0,CurrentHealth);
        if(amount>0)
            CurrentHealth -= amount;
    }
    public void AddHealth(float amount)
    {
        if(amount>0)
            CurrentHealth = Mathf.Clamp(CurrentHealth+amount,0,MaxHealth) ;
    }
    private void OnDestroy()
    {
       
    }
}

public enum Faction
{
    GoodGuys,
    BadGuys,
}