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
    [SerializeField] private float minSpawnGap = 0.5f;
    private float lastSpawnTime;

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

        // starterCars();
    }

    float RandomXPosition()
    {
        return Random.Range(-xSpawnRange, xSpawnRange);
    }

    void SpawnFromPool(ObjectPool pool, float zPos)
    {
        if (pool == null) return;

        GameObject obj = pool.GetObject();

        if (obj != null)
        {
            Vector3 spawnPos = new(RandomXPosition(), 0, zPos);
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

    void starterCars()
    {
        for (int i = 1; i <= 3; i++)
        {
            float zPosition = zSpawn - (i * 15f);

            int randomIndex = Random.Range(0, carPools.Count);
            ObjectPool selectedPool = carPools[randomIndex];

            GameObject car = selectedPool.GetObject();

            if (car != null)
            {
                Vector3 spawnPos = new(
                    RandomXPosition(),
                    car.transform.position.y,
                    zPosition
                );

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
            SpawnRandomCar();

            float interval = GetCarSpawnInterval();
            interval += Random.Range(0.1f, 0.4f);

            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator SpawnBarrierRoutine(float startDelay)
    {
        while (!stopSpawning)
        {
            yield return new WaitForSeconds(startDelay);
            SpawnBarrier();

            float interval = GetCarSpawnInterval();
            interval += Random.Range(0.1f, 0.4f);

            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator SpawnPowerupRoutine(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        while (!stopSpawning)
        {
            SpawnPowerup();

            float interval = GetCarSpawnInterval();
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
        float maxSpeed = GameManager.Instance.maxSpeed;

        return Mathf.Clamp01(speed / maxSpeed);
    }

    float GetCarSpawnInterval()
    {
        float t = DifficultyPercent();
        return Mathf.Lerp(baseCarInterval, 0.8f, t);
    }

    float GetBarrierSpawnInterval()
    {
        float t = DifficultyPercent();
        return Mathf.Lerp(baseBarrierInterval, 2.5f, t);
    }

    float GetPowerupSpawnInterval()
    {
        float t = DifficultyPercent();

        // Powerups ficam um pouco mais raros com dificuldade
        return Mathf.Lerp(basePowerupInterval, basePowerupInterval + 4f, t);
    }
}