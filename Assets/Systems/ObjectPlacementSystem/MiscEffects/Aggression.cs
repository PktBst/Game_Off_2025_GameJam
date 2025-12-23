using UnityEngine;

public class Aggression : MiscEffect
{
    public override bool TryUsingThisCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var post = hit.transform.GetComponentInParent<SquadPost>();
            if (post!=null)
            {
                foreach(var unit in post.UnitList)
                {
                    if(unit.TryGetComponent(out StatsComponent stats))
                    {
                        stats.BaseAttackPoints += (stats.BaseAttackPoints * 0.2f);
                    }
                }
                return true;
            }
        }
        return false;
    }

    public override void Init()
    {
        
    }
}
