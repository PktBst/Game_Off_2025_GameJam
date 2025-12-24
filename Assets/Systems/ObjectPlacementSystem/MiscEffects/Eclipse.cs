using UnityEngine;

public class Eclipse : MiscEffect
{
    public override bool TryUsingThisCard()
    {
        if (GridSystem.Instance?.CurrentTile == null) return false;

        var hits = Physics.OverlapBox(GridSystem.Instance.CurrentTile.Pos, Vector3.one * 0.5f);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out SquadPost post))
            {
                foreach (var unit in post.UnitList)
                {
                    if (unit.gameObject.TryGetComponent(out StatsComponent stats))
                    {
                        stats.BaseAttackPoints *= 2;
                    }
                }
                if (AnimationPool.Instance != null)
                {
                    AnimationPool.Instance.Play_CFXR2_Shiny_Item_Loop__AnimationAtFor(post.transform.forward, post.transform.position, 0.7f);
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