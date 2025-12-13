using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    public StatsComponent Stats;
    float MaxHealth;
    float CurrentHealth;

    [SerializeField] Image FillImage;

    public void Init(float MaxHealth)
    {
        CurrentHealth = MaxHealth;
        this.MaxHealth = MaxHealth;
        GameManager.Instance.TickSystem.Subscribe(updateUI);
        Stats = GetComponent<StatsComponent>();
    }

    void updateUI()
    {
        CurrentHealth -=Time.deltaTime;
        FillImage.fillAmount = CurrentHealth/MaxHealth;
        if (CurrentHealth<=0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.TickSystem.Unsubscribe(updateUI);
    }
}

public enum Faction
{
    GoodGuys,
    BadGuys
}