using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BadGuysSpawner : MonoBehaviour
{
    [Header("Prefabs & Locations")]
    [SerializeField] private List<StatsComponent> badguysPrefabs;
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Spawn Settings")]
    [SerializeField] private bool spawnAtDay;
    [SerializeField] private bool spawnAtNight;
    [SerializeField] private int badguyCountInWave = 5;
    [SerializeField] private int wavesToSpawn = 3;
    [SerializeField] private float spawnDelay = 0.5f;

    [Header("Prediction Settings")]
    [SerializeField] private bool predictAtDay;
    [SerializeField] private bool predictAtNight;
    [SerializeField] private TextMeshProUGUI wavePredictionText;

    private readonly List<StatsComponent> spawnedBadguys = new();
    private readonly List<Vector3> predictedSpawnPositions = new();

    private int remainingInCurrentWave;
    private int completedWaves;
    private int spawnIndex;
    private float lastSpawnTime = -100f;

    private bool predictionIssued;
    private TimeOfDay? waveTime;

    private void Start()
    {
        ResetState();

        if (GameManager.Instance?.TickSystem != null)
            GameManager.Instance.TickSystem.Subscribe(TickUpdate);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance?.TickSystem != null)
            GameManager.Instance.TickSystem.Unsubscribe(TickUpdate);
    }

    private void TickUpdate()
    {
        if (DayNightCycleCounter.Instance == null) return;

        TimeOfDay currentTime = DayNightCycleCounter.Instance.CurrentTime;

        if (waveTime.HasValue && currentTime != waveTime.Value)
        {
            ResetState();
            return;
        }

        bool isPredictionTime =
            (predictAtDay && currentTime == TimeOfDay.Day) ||
            (predictAtNight && currentTime == TimeOfDay.Night);

        if (isPredictionTime && !predictionIssued)
        {
            PredictNextWave();
            predictionIssued = true;
        }

        bool isSpawnTime =
            (spawnAtDay && currentTime == TimeOfDay.Day) ||
            (spawnAtNight && currentTime == TimeOfDay.Night);

        if (!isSpawnTime || !predictionIssued)
            return;

        // Prevent over-spawning for this time period
        if (completedWaves >= wavesToSpawn &&
            remainingInCurrentWave <= 0 &&
            spawnedBadguys.Count == 0)
        {
            return;
        }

        // Start new wave
        if (remainingInCurrentWave <= 0 && completedWaves < wavesToSpawn)
        {
            int waveNumber = completedWaves + 1;

            // Base scaling
            int baseCount = badguyCountInWave * waveNumber;

            // Controlled randomness (±20%)
            int variance = Mathf.Max(1, baseCount / 5);
            int randomizedCount = baseCount + Random.Range(-variance, variance + 1);

            remainingInCurrentWave = Mathf.Max(1, randomizedCount);

            completedWaves++;
            spawnIndex = 0;
            waveTime = currentTime;

            DayNightCycleCounter.Instance.PausePhasing();
        }


        // Spawn individual enemies
        if (remainingInCurrentWave > 0 &&
            Time.time >= lastSpawnTime + spawnDelay)
        {
            SpawnEnemy();
        }
    }
    private void PredictNextWave()
    {
        predictedSpawnPositions.Clear();

        if (spawnPoints.Count == 0) return;

        int amount = Mathf.Clamp(Random.Range(1, 3), 1, spawnPoints.Count);
        HashSet<int> usedIndices = new();

        for (int i = 0; i < amount; i++)
        {
            int idx;
            do
            {
                idx = Random.Range(0, spawnPoints.Count);
            }
            while (usedIndices.Contains(idx));

            usedIndices.Add(idx);
            predictedSpawnPositions.Add(spawnPoints[idx].position);
        }

        UpdateWavePredictionText(predictedSpawnPositions);
    }

    private void UpdateWavePredictionText(List<Vector3> positions)
    {
        if (wavePredictionText == null || positions.Count == 0) return;

        HashSet<string> directions = new();

        foreach (var pos in positions)
        {
            Vector3 dir = pos.normalized;

            float angle = Mathf.Atan2(-dir.x, dir.z) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            string direction = angle switch
            {
                >= 337.5f or < 22.5f => "North",
                >= 22.5f and < 67.5f => "North-East",
                >= 67.5f and < 112.5f => "East",
                >= 112.5f and < 157.5f => "South-East",
                >= 157.5f and < 202.5f => "South",
                >= 202.5f and < 247.5f => "South-West",
                >= 247.5f and < 292.5f => "West",
                >= 292.5f and < 337.5f => "North-West",
                _ => null
            };

            if (direction != null)
                directions.Add(direction);
        }

        wavePredictionText.SetText(
            $"Incoming wave from {string.Join(" & ", directions)}"
        );
    }
    private void SpawnEnemy()
    {
        if (badguysPrefabs.Count == 0 || predictedSpawnPositions.Count == 0)
            return;

        int enemyIndex = Random.Range(0, badguysPrefabs.Count);

        Vector3 spawnPos = predictedSpawnPositions[spawnIndex];
        spawnIndex = (spawnIndex + 1) % predictedSpawnPositions.Count;

        var badGuy = Instantiate(
            badguysPrefabs[enemyIndex],
            spawnPos,
            Quaternion.identity
        );

        spawnedBadguys.Add(badGuy);

        var health = badGuy.GetComponent<HealthComponent>();
        if (health != null)
            health.OnDeath += () => HandleEnemyDeath(badGuy);

        lastSpawnTime = Time.time;
        remainingInCurrentWave--;
    }

    private void HandleEnemyDeath(StatsComponent enemy)
    {
        spawnedBadguys.Remove(enemy);

        if (spawnedBadguys.Count == 0 &&
            remainingInCurrentWave <= 0 &&
            completedWaves >= wavesToSpawn)
        {
            wavePredictionText?.SetText("");
            predictedSpawnPositions.Clear();
            predictionIssued = false;
            StartCoroutine(OnLootMenuAndHandleDayPhases());
            //GameManager.Instance?.CardSystem.openLootMenu();

            //DayNightCycleCounter.Instance?.ResumePhasing();
            //if (!DayNightCycleCounter.Instance.IsAutomatic)
            //{
            //    DayNightCycleCounter.Instance.SetDay();
            //}
        }
    }

    System.Collections.IEnumerator OnLootMenuAndHandleDayPhases()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance?.CardSystem.openLootMenu();
        while (GameManager.Instance.CardSystem.IsLootMenuOpen)
        {
            yield return null;
        }
        yield return null;
        DayNightCycleCounter.Instance?.ResumePhasing();
        if (!DayNightCycleCounter.Instance.IsAutomatic)
        {
            DayNightCycleCounter.Instance.SetDay();
        }
    }

    private void ResetState()
    {
        completedWaves = 0;
        remainingInCurrentWave = 0;
        spawnIndex = 0;
        predictionIssued = false;
        waveTime = null;

        predictedSpawnPositions.Clear();
        wavePredictionText?.SetText("");
    }
}
