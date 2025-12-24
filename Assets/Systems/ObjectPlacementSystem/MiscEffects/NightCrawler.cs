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
            Cavemen post = hit.GetComponentInParent<Cavemen>();
            if (post!=null)
            {
                ComponentUtility.AddComponentWithValues(post.gameObject, this).subscribe(post);
                return true;
            }
        }
        return false;
    }

    void subscribe(Cavemen post)
    {
        post.OnUnitDie += (unit) => { RespawnAndKillUnitAfter1Sec(post, unit); };
        if (AnimationPool.Instance != null)
        {
            AnimationPool.Instance.Play_CFXR2_Shiny_Item_Loop__AnimationAtFor(post.Post.SquadPostPiller.transform.forward, post.Post.SquadPostPiller.transform.position, 0.7f);
        }
    }

    void RespawnAndKillUnitAfter1Sec(Cavemen post, NavMeshAgent unit)
    {
        Debug.Log("[NightCrawler tried to revive unit]");
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
