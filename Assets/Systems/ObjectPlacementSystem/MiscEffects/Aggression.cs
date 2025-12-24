using UnityEngine;

public class Aggression : MiscEffect
{
    public override bool TryUsingThisCard()
    {
        if(GridSystem.Instance?.CurrentTile==null)return false;

        var hits = Physics.OverlapBox(GridSystem.Instance.CurrentTile.Pos, Vector3.one * 0.5f);
        foreach(var hit in hits)
        {
            SquadPost post = hit.GetComponentInParent<SquadPost>();
            if (post!=null)
            {
                foreach (var unit in post.UnitList)
                {
                    if (unit.gameObject.TryGetComponent(out StatsComponent stats))
                    {
                        stats.BaseAttackPoints += (stats.BaseAttackPoints * 0.2f);
                    }
                }
                if (AnimationPool.Instance != null)
                {
                    AnimationPool.Instance.Play_CFXR2_Shiny_Item_Loop__AnimationAtFor(post.SquadPostPiller.transform.forward, post.SquadPostPiller.transform.position, 0.7f);
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
