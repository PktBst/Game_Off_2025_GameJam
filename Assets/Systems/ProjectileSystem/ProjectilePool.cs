using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{

    [SerializeField] private Projectile projectilePrefab;
    List<Projectile> projectiles = new();
    public static ProjectilePool Instance;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        for(int i = 0; i < 25; i++)
        {
            SpawnNewProjectile();
        }
    }

    public Projectile GetProjectile()
    {
        foreach(var projectile in projectiles)
        {
            if (!projectile.gameObject.activeSelf)
            {
                return projectile;
            }
        }
        return SpawnNewProjectile();

    }

    Projectile SpawnNewProjectile()
    {
        var newProjectile = Instantiate(projectilePrefab, transform);
        newProjectile.Deactivate();
        projectiles.Add(newProjectile);
        return newProjectile;
    }
}
