using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

using System.IO;


#if UNITY_EDITOR
using UnityEditor;
#endif
public class AnimationPool : MonoBehaviour
{
    [SerializeField] List<GameObject> animationPrefabs;

    Dictionary<string,GameObject> animationPrefabsDict;

    Dictionary<string, List<GameObject>> animationPool;

    [SerializeField] private int poolSize = 5;

    public static AnimationPool Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitPool();
        }
        else
        {
            Destroy(this);
        }
    }

    private void InitPool()
    {
        if(animationPrefabs ==  null)
        {
            Debug.Log("[Animation Pool] there are no prefabs attached in the inspector");
            return;
        }

        animationPrefabsDict = new();
        foreach (var animationPrefab in animationPrefabs)
        {
            animationPrefabsDict.Add(RemoveWhitespace(animationPrefab.name), animationPrefab);
        }

        animationPool = new();
        foreach (var animationPoolPrefabKVP in animationPrefabsDict)
        {
            if (!animationPool.TryGetValue(animationPoolPrefabKVP.Key, out var prefabPool))
            {
                prefabPool = new();
                for (int i = 0; i < poolSize; i++)
                {
                    var spawnedPrefab = Instantiate(animationPoolPrefabKVP.Value, transform);
                    prefabPool.Add(spawnedPrefab);
                    spawnedPrefab.SetActive(false);
                }
            }
        }
    }
    #region animationFunctions
    public Coroutine Play_CFXR2_Skull_Head_Alt_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2SkullHeadAlt", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Souls_Escape_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2SoulsEscape", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_WW_Enemy_Explosion_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2WWEnemyExplosion", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Electrified_3_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRElectrified3", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Sparks_Rain_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2SparksRain", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Hit_Electric_C_Air__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3HitElectricC(Air)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Explosion_1_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRExplosion1", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Explosion_Smoke_2_Solo_HDR__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRExplosionSmoke2Solo(HDR)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_WW_Explosion_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2WWExplosion", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Fire_Explosion_B_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3FireExplosionB", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR4_Firework_1_Cyan_Purple_HDR__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR4Firework1Cyan-Purple(HDR)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR4_Firework_HDR_Shoot_Single_Random_Color__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR4FireworkHDRShootSingle(RandomColor)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Fire_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRFire", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Fire_Breath_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRFireBreath", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Firewall_A_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2FirewallA", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Hit_Fire_B_Air__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3HitFireB(Air)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR4_Sun_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR4Sun", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Hit_Ice_B_Air__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3HitIceB(Air)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Hit_A_Red__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRHitA(Red)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Hit_D_3D_Yellow__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRHitD3D(Yellow)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Impact_Glowing_HDR_Blue__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRImpactGlowingHDR(Blue)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Ground_Hit_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2GroundHit", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Hit_Light_B_Air__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3HitLightB(Air)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_LightGlow_A_Loop__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3LightGlowA(Loop)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Water_Ripples_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRWaterRipples", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Water_Splash_Smaller__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRWaterSplash(Smaller)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Blood_Directional__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2Blood(Directional)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Blood_Shape_Splash_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2BloodShapeSplash", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR4_Bubbles_Breath_Underwater_Loop_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR4BubblesBreathUnderwaterLoop", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Magic_Aura_A_Runic__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3MagicAuraA(Runic)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR4_Bouncing_Glows_Bubble_Blue_Purple__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR4BouncingGlowsBubble(BluePurple)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR4_Falling_Stars_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR4FallingStars", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Flash_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRFlash", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Magic_Poof_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRMagicPoof", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR_Smoke_Source_3D_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXRSmokeSource3D", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Broken_Heart_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2BrokenHeart", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Cartoon_Fight_Loop__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2CartoonFight(Loop)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Poison_Cloud_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2PoisonCloud", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR2_Shiny_Item_Loop__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR2ShinyItem(Loop)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Ambient_Glows_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3AmbientGlows", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Hit_Misc_A_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3HitMiscA", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Hit_Misc_F_Smoke_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3HitMiscFSmoke", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Hit_Leaves_A_Lit__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3HitLeavesA(Lit)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR3_Shield_Leaves_A_Lit__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR3ShieldLeavesA(Lit)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR4_Rain_Falling_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR4RainFalling", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR4_Rain_Splashes_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR4RainSplashes", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_CFXR4_Wind_Trails_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("CFXR4WindTrails", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_BImpact_Dirt_Hole_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_BImpactDirt+Hole", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_BImpact_Sand_Hole_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_BImpactSand+Hole", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_BImpact_SoftBody_Hole_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_BImpactSoftBody+Hole", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Explosion_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_Explosion", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Explosion_LandMine_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosionLandMine", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Explosion_Simple_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosionSimple", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Explosion_Small_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosionSmall", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Explosion_StarSmoke_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosionStarSmoke", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_ExplosiveSmoke_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosiveSmoke", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_ExplosiveSmoke_Big_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosiveSmokeBig", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_ExplosiveSmoke_Big_Alt_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosiveSmokeBigAlt", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_ExplosiveSmoke_Small_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosiveSmokeSmall", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_ExplosiveSmokeGround_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosiveSmokeGround", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_ExplosiveSmokeGround_Big_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosiveSmokeGroundBig", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_ExplosiveSmokeGround_Big_Alt_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosiveSmokeGroundBigAlt", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_ExplosiveSmokeGround_Small_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_ExplosiveSmokeGroundSmall", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Nuke_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_Nuke", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_FlameThrower_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FlameThrower", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_FlameThrower_Big_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FlameThrowerBig", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_FlameThrower_Big_Alt_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FlameThrowerBigAlt", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_FlameThrower_Big_Alt_Looped_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FlameThrowerBigAltLooped", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_FlameThrower_Big_Looped_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FlameThrowerBigLooped", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_FlameThrower_Looped_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FlameThrowerLooped", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_GazFireBig_Blue__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_GazFireBig(Blue)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_GazFireBig_Green__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_GazFireBig(Green)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_GazFireBig_Purple__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_GazFireBig(Purple)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_Natural_Black_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireNatural(BlackSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_Natural_Broad_Black_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireNaturalBroad(BlackSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_Natural_Circle_Black_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireNaturalCircle(BlackSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_SmallFlame_Black_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireSmallFlame(BlackSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_GazFire_Black_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_GazFire(BlackSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_GazFireBig_Black_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_GazFireBig(BlackSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_Natural_Gray_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireNatural(GraySmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_Natural_Broad_Gray_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireNaturalBroad(GraySmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_Natural_Circle_Gray_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireNaturalCircle(GraySmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_SmallFlame_Gray_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireSmallFlame(GraySmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_GazFire_Gray_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_GazFire(GraySmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_GazFireBig_Gray_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_GazFireBig(GraySmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_Natural_No_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireNatural(NoSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_Natural_Broad_No_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireNaturalBroad(NoSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_Natural_Circle_No_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireNaturalCircle(NoSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_Fire_SmallFlame_No_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_FireSmallFlame(NoSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_GazFire_No_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_GazFire(NoSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_GazFireBig_No_Smoke__AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_GazFireBig(NoSmoke)", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_MF_4P_RIFLE1_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_MF4PRIFLE1", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_MF_4P_RIFLE2_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_MF4PRIFLE2", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_MF_4P_RIFLE3_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_MF4PRIFLE3", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_MF_FPS_RIFLE1_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_MFFPSRIFLE1", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_MF_FPS_RIFLE2_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_MFFPSRIFLE2", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_MF_FPS_RIFLE3_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_MFFPSRIFLE3", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_Additive_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeAdditive", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_AlphaBlend_Black_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeAlphaBlendBlack", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_AlphaBlend_Gray_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeAlphaBlendGray", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_Black_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeBlack", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_Gray_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeGray", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_White_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeWhite", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_AlphaBlend_Blue_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeAlphaBlendBlue", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_AlphaBlend_Green_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeAlphaBlendGreen", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_AlphaBlend_Purple_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeAlphaBlendPurple", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_AlphaBlend_Red_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeAlphaBlendRed", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_Blue_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeBlue", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_Green_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeGreen", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_Red_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeRed", forwardDirection, position, timeDurationInSeconds);
    }

    public Coroutine Play_WFXMR_SmokeGrenade_Yellow_AnimationAtFor(Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)
    {
        return PlayAnimationAtFor("WFXMR_SmokeGrenadeYellow", forwardDirection, position, timeDurationInSeconds);
    }
    #endregion

    [Button]
    public void WriteFunctions()
    {
        if (animationPrefabs == null || animationPrefabs.Count == 0)
        {
            Debug.Log("[Animation Pool] there are no prefabs attached in the inspector");
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (var animationPrefab in animationPrefabs)
        {
            if (animationPrefab == null)
                continue;

            string nameWithoutWhiteSpace = RemoveWhitespace(animationPrefab.name);
            string methodName = ToValidCSharpIdentifierClean(animationPrefab.name);

            sb.AppendLine($"public Coroutine Play_{methodName}_AnimationAtFor(" +
                          $"Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds = 0.5f)");
            sb.AppendLine("{");
            sb.AppendLine($"    return PlayAnimationAtFor(\"{nameWithoutWhiteSpace}\", forwardDirection, position, timeDurationInSeconds);");
            sb.AppendLine("}");
            sb.AppendLine();
        }

#if UNITY_EDITOR
        string folderPath = "Assets/Generated";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, "animationFunctions.txt");
        File.WriteAllText(filePath, sb.ToString());

        AssetDatabase.Refresh();
#endif

        Debug.Log("[Animation Pool] animationFunctions.txt generated successfully");
    }
    public static string ToValidCSharpIdentifierClean(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "_";

        Span<char> buffer = stackalloc char[input.Length + 1];
        int index = 0;
        bool lastUnderscore = false;

        foreach (char c in input)
        {
            char outChar =
                char.IsLetterOrDigit(c) || c == '_'
                ? c
                : '_';

            if (index == 0 && char.IsDigit(outChar))
                outChar = '_';

            if (outChar == '_' && lastUnderscore)
                continue;

            buffer[index++] = outChar;
            lastUnderscore = outChar == '_';
        }

        return new string(buffer[..index]);
    }




    public Coroutine PlayAnimationAtFor(string name, Vector3 forwardDirection, Vector3 position, float timeDurationInSeconds)
    {
        if (!TryAndGetPooledAnimationByName(name, out var animationInstance))
        {
            Debug.LogWarning($"[Animation Pool] Animation '{name}' not found in pool. Spawning a new one.");

            // Try to get prefab from dictionary
            if (animationPrefabsDict != null && animationPrefabsDict.TryGetValue(name, out var prefab))
            {
                // Instantiate new animation and add it to the pool
                animationInstance = Instantiate(prefab, transform);

                if (!animationPool.TryGetValue(name, out var prefabPool))
                {
                    prefabPool = new List<GameObject>();
                    animationPool.Add(name, prefabPool);
                }

                prefabPool.Add(animationInstance);

                // Deactivate initially so pooling logic is consistent
                animationInstance.SetActive(false);
            }
            else
            {
                Debug.LogError($"[Animation Pool] No prefab found for animation '{name}' in dictionary.");
                return StartCoroutine(PlayAnimationFor(null, timeDurationInSeconds));
            }
        }

        // Set position
        animationInstance.transform.position = position;

        // Set forward direction safely
        if (forwardDirection.sqrMagnitude > 0.000001f)
        {
            animationInstance.transform.forward = forwardDirection.normalized;
        }

        // Start playing the animation for the given duration
        return StartCoroutine(PlayAnimationFor(animationInstance, timeDurationInSeconds));
    }



    System.Collections.IEnumerator PlayAnimationFor(GameObject animationInstance, float timeDurationInSeconds)
    {
        if(animationInstance == null)
        {
            yield break;
        }
        animationInstance.SetActive(true);

        var ps = animationInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Clear(true); // Clear old particles
            ps.Play(true);  // Play system
        }
        Debug.Log($"{animationInstance.name} was played at {animationInstance.transform.position}");
        yield return new WaitForSeconds(timeDurationInSeconds);

        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        animationInstance.SetActive(false);
    }



    public bool TryAndGetPooledAnimationByName(string name, out GameObject animationInstance)
    {
        animationInstance = null;
        name = RemoveWhitespace(name);
        if(!animationPool.TryGetValue(name, out var prefabPool))
        {
            return false;
        }
        foreach(var alreadySpawnedPrefab in prefabPool)
        {
            if (alreadySpawnedPrefab.activeInHierarchy)
            {
                animationInstance = alreadySpawnedPrefab;
                return true;
            }
        }

        animationInstance = Instantiate(animationPrefabsDict[name], transform);
        prefabPool.Add(animationInstance);
        animationInstance.SetActive(false);
        return true;
    }

    public static string RemoveWhitespace(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        Span<char> buffer = stackalloc char[input.Length];
        int index = 0;

        foreach (char c in input)
        {
            if (!char.IsWhiteSpace(c))
                buffer[index++] = c;
        }
        return new string(buffer[..index]);
    }


}
