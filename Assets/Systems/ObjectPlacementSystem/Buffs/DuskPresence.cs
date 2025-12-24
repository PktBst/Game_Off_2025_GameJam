using UnityEngine;

public class DuskPresence : BuffEffect
{
    public override void Init()
    {
        if(TryGetComponent(out StatsComponent stats))
        {
            stats.TaxAmount += 2;
        }
    }
}
