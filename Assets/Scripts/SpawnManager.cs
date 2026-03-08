using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool stopSpawning = false;

    public List<ObjectPool> carPools;
    public ObjectPool barrierPool;
    public ObjectPool powerupPool;

    [SerializeField] private float xSpawnRange = 2.5f;
    [SerializeField] private float zSpawn = 60f;
    [SerializeField] private float minSpawnGap = 0.4f;
    [SerializeField] private float[] lanes = { -2.7f, -0.9f, 0.9f, 2.7f };
    private float lastSpawnTime;
    private int lastLane = -1;

    [Header("Base Spawn Intervals")]
    [SerializeField] private float baseCarInterval = 3f;
    [SerializeField] private float baseBarrierInterval = 6f;
    [SerializeField] private float basePowerupInterval = 12f;
    [Header("Start Delays")]
    [SerializeField] private float carStartDelay = 0f;
    [SerializeField] private float barrierStartDelay = 1.5f;
    [SerializeField] private float powerupStartDelay = 3f;


    private void Start()
    {
        if (stopSpawning) return;

        StartCoroutine(SpawnCarsRoutine(carStartDelay));
        StartCoroutine(SpawnBarrierRoutine(barrierStartDelay));
        StartCoroutine(SpawnPowerupRoutine(powerupStartDelay));

        StarterCars();
    }
    private void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            stopSpawning = true;
            StopAllCoroutines();

        }
    }

    float RandomLane()
    {
        int lane;

        do
        {
            lane = Random.Range(0, lanes.Length);
        }
        while (lane == lastLane);

        lastLane = lane;

        return lanes[lane];
    }

    void SpawnFromPool(ObjectPool pool, float zPos)
    {
        if (pool == null) return;

        GameObject obj = pool.GetObject();

        if (obj != null)
        {
            Vector3 spawnPos = new(RandomLane(), 0, zPos);
            obj.transform.position = spawnPos;
        }
    }

    void SpawnRandomCar()
    {
        if (carPools.Count == 0) return;

        int randomIndex = Random.Range(0, carPools.Count);
        ObjectPool selectedPool = carPools[randomIndex];

        SpawnFromPool(selectedPool, zSpawn);
    }

    void SpawnBarrier()
    {
        SpawnFromPool(barrierPool, zSpawn);
    }

    void SpawnPowerup()
    {
        SpawnFromPool(powerupPool, zSpawn);
    }

    void StarterCars()
    {
        for (int i = 1; i <= 3; i++)
        {
            float zPosition = zSpawn - (i * 15f);

            int randomIndex = Random.Range(0, carPools.Count);
            ObjectPool selectedPool = carPools[randomIndex];

            GameObject car = selectedPool.GetObject();

            if (car != null)
            {
                Vector3 spawnPos = new(RandomLane(), 0, zPosition);

                car.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
            }
        }
    }

    // =========================
    // COROUTINES
    // =========================
    IEnumerator WaitForSpawnGap()
    {
        float wait = Mathf.Max(0, minSpawnGap - (Time.time - lastSpawnTime));

        if (wait > 0)
            yield return new WaitForSeconds(wait);

        lastSpawnTime = Time.time;
    }
    IEnumerator SpawnCarsRoutine(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        while (!stopSpawning)
        {
            yield return WaitForSpawnGap();

            SpawnRandomCar();

            float interval = GetCarSpawnInterval();
            interval += Random.Range(0.1f, 0.4f);

            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator SpawnBarrierRoutine(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        while (!stopSpawning)
        {
            yield return WaitForSpawnGap();

            SpawnBarrier();

            float interval = GetBarrierSpawnInterval();
            interval += Random.Range(0.1f, 0.4f);

            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator SpawnPowerupRoutine(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        while (!stopSpawning)
        {
            yield return WaitForSpawnGap();

            SpawnPowerup();

            float interval = GetPowerupSpawnInterval();
            interval += Random.Range(0.1f, 0.4f);

            yield return new WaitForSeconds(interval);
        }
    }

    // =========================
    // INTERVAL CALCULATIONS
    // =========================

    float DifficultyPercent()
    {
        float speed = GameManager.Instance.speed;
        float baseSpeed = GameManager.Instance.baseSpeed;
        float maxSpeed = GameManager.Instance.maxSpeed;

        return Mathf.InverseLerp(baseSpeed, maxSpeed, speed);
    }

    float GetCarSpawnInterval()
    {
        float t = DifficultyPercent();
        return Mathf.Lerp(baseCarInterval, 0.5f, t);
    }

    float GetBarrierSpawnInterval()
    {
        float t = DifficultyPercent();
        return Mathf.Lerp(baseBarrierInterval, 1.5f, t);
    }

    float GetPowerupSpawnInterval()
    {
        float difficultyT = DifficultyPercent();

        float fuelPercent = GameManager.Instance.fuel / 100;

        float baseInterval = Mathf.Lerp(8f, 1f, difficultyT);

        // Se combustível baixo, ajuda o jogador
        float fuelFactor = Mathf.Lerp(0.5f, 1f, fuelPercent);

        return baseInterval * fuelFactor;
    }
}