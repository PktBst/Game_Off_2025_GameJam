using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NightCrawler : MiscEffect
{
    public override bool TryUsingThisCard()
    {
        if (GridSystem.Instance?.CurrentTile == null) return false;

        var hits = Physics.OverlapBox(GridSystem.Instance.CurrentTile.Pos, Vector3.one * 0.5f);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Cavemen post))
            {
                post.OnUnitDie += (unit)=> { RespawnAndKillUnitAfter1Sec(post, unit); };
                if (AnimationPool.Instance != null)
                {
                    AnimationPool.Instance.Play_CFXR2_Shiny_Item_Loop__AnimationAtFor(post.transform.forward, post.transform.position, 0.7f);
                }
                return true;
            }
        }
        return false;
    }

    void RespawnAndKillUnitAfter1Sec(Cavemen post, NavMeshAgent unit)
    {
        StartCoroutine(ReSpawnUnit(post,unit.transform.position));
    }

    IEnumerator ReSpawnUnit(Cavemen post, Vector3 pos)
    {
        yield return new WaitForEndOfFrame();
        var unit = post.SpawnUnitAt(pos);

        yield return new WaitForSeconds(5);
        if(unit.TryGetComponent(out HealthComponent health))
        {
            health.DeductHealth(health.MaxHealth);
        }

    }

    public override void Init()
    {

    }
}
