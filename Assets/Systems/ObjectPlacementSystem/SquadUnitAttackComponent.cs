using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class SquadUnitAttackComponent : MonoBehaviour
{
    private NavMeshAgent agent;
    private SquadPost SquadPost;
    private StatsComponent Stat;
    public HealthComponent target;
    public ParticleSystem Attacksfx;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SquadPost = transform.parent.GetComponent<SquadPost>();
        Stat = GetComponent<StatsComponent>();
        Attacksfx = transform.GetChild(2).GetComponent<ParticleSystem>();
        Attacksfx.Stop();

    }

    // Declare a timer variable and a time interval
    private float timer = 0f;
    private float updateInterval = 0.25f; // 1 second / 4 times = 0.25 seconds per update

    private void Update()
    {
        timer += Time.deltaTime; // Increase the timer by the time since the last frame

        if (timer >= updateInterval)
        {
            // Reset the timer after it reaches the interval
            timer = 0f;

            // Your logic to run every 0.25 seconds (4 times per second)
            if (Vector3.Distance(transform.position, SquadPost.SquadPostPiller.position) < 1.5f)
            {
                //if (target != null) return;
                Attacksfx.Stop();
                SearchForEnemy();
            }
            else
            {
                target = null;
                agent.SetDestination(SquadPost.SquadPostPiller.transform.position);
            }
        }
    }

    void SearchForEnemy()
    {

            RaycastHit[] hits = Physics.SphereCastAll(SquadPost.SquadPostPiller.position, SquadPost.followTriggerRange, Vector3.up, 0f);

            foreach (RaycastHit hit in hits)
            {
                HealthComponent targetHealth = hit.collider.GetComponent<HealthComponent>();

                if (targetHealth != null && targetHealth.Stats.FactionType == Faction.BadGuys)
                {
                    target = targetHealth;
                    MoveCloserToEnemy();
                    break;
                }
            }
       

    }

    // Gizmo drawing method for the sphere
    void OnDrawGizmos()
    {
        if (SquadPost != null)
        {
            Gizmos.color = Color.red; // Color of the sphere
            Gizmos.DrawWireSphere(SquadPost.SquadPostPiller.position, SquadPost.followTriggerRange); // Draw the sphere at SquadPost position
        }
        if(target != null)
        {
            Gizmos.color = Color.yellow;  // Set the line color (you can change this)
            Gizmos.DrawLine(transform.position, target.transform.position);  // Draw the line from this object to the target
        }
    }

    void MoveCloserToEnemy()
    {
        if (Vector3.Distance(target.transform.position, transform.position) > 0.4f)
        {
            agent.SetDestination(target.transform.position);
            
        }
        else
        {
            Attack();
        }
    }

    void Attack()
    {
        target.DeductHealth(Stat.BaseAttackPoints);
        Attacksfx.Play();
        Debug.Log("Target within range: Attacking " + target.transform.name);
    }




}
