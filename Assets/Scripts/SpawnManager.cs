using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool stopSpawning = false;
    public List<ObjectPool> carPools;
    public ObjectPool barrelPool;
    public ObjectPool powerupPool;

    private readonly float xSpawnRange = 2.5f;
    private float zSpawn = 60.0f;
    private readonly float delaySpawn = 0f;
    private readonly float repeatSpawnCar = 3f;
    private readonly float repeatSpawnBarrel = 6f;
    private readonly float repeatSpawnPowerup = 12f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (stopSpawning)
            return;
        InvokeRepeating(nameof(SpawnRandomCar), delaySpawn, repeatSpawnCar);
        InvokeRepeating(nameof(SpawnBarrel), delaySpawn + 1f, repeatSpawnBarrel);
        InvokeRepeating(nameof(SpawnPowerup), delaySpawn + 2f, repeatSpawnPowerup);
        //starterCars();

    }

    void Update()
    {
        if (stopSpawning)
            CancelInvoke();
    }
    float RandomXPosition()
    {
        return Random.Range(-xSpawnRange, xSpawnRange);
    }
    void SpawnFromPool(ObjectPool pool, float zPos)
    {
        GameObject obj = pool.GetObject();

        if (obj != null)
        {
            Vector3 spawnPos = new(RandomXPosition(), obj.transform.position.y, zPos);
            obj.transform.position = spawnPos;
        }
    }
    void SpawnRandomCar()
    {
        if (carPools.Count == 0) return;

        int randomIndex = Random.Range(0, carPools.Count);
        ObjectPool selectedPool = carPools[randomIndex];

        GameObject car = selectedPool.GetObject();

        if (car != null)
        {
            
            MoveForward move = car.GetComponent<MoveForward>();
            move.SetPool(selectedPool);

            Vector3 spawnPos = new(RandomXPosition(), car.transform.position.y, zSpawn);
            car.transform.position = spawnPos;
        }
    }
    void SpawnBarrel()
    {
        SpawnFromPool(barrelPool, zSpawn);
    }
    void SpawnPowerup()
    {
        SpawnFromPool(powerupPool, zSpawn);
    }
    void starterCars()
    {
        for (int i = 1; i < 4; i++)
        {
            float zPosition = i * 15f;

            int randomIndex = Random.Range(0, carPools.Count);
            ObjectPool selectedPool = carPools[randomIndex];

            GameObject car = selectedPool.GetObject();

            if (car != null)
            {
                Vector3 spawnPos = new(RandomXPosition(), car.transform.position.y, zPosition);
                car.transform.position = spawnPos;
            }
        }
    }
}
