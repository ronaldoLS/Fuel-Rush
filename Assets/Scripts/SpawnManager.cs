using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool stopSpawning = false;
    public GameObject[] CarsPrefab;
    public GameObject barrelPrefab;
    public GameObject powerupPrefab;



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
        starterCars();

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
    void SpawnRandomCar()
    {
        int randomIndex = Random.Range(0, CarsPrefab.Length);
        GameObject carPrefab = CarsPrefab[randomIndex];
        Vector3 carPrefabPosition = carPrefab.transform.position;
        Vector3 spawnPos = new(RandomXPosition(), carPrefabPosition.y, zSpawn);
        Instantiate(CarsPrefab[randomIndex], spawnPos, CarsPrefab[randomIndex].transform.rotation);
    }
    void SpawnBarrel()
    {
        Vector3 barrelPrefabPosition = barrelPrefab.transform.position;
        Vector3 spawnPos = new(RandomXPosition(), barrelPrefabPosition.y, zSpawn);
        Instantiate(barrelPrefab, spawnPos, barrelPrefab.transform.rotation);
    }
    void SpawnPowerup()
    {
        Vector3 powerupPrefabPosition = powerupPrefab.transform.position;
        Vector3 spawnPos = new(RandomXPosition(), powerupPrefabPosition.y, zSpawn);
        Instantiate(powerupPrefab, spawnPos, powerupPrefab.transform.rotation);
    }
    void starterCars()
    {
        for (int i = 1; i < 4; i++)
        {
            float zPosition = i * 15f;
            int randomIndex = Random.Range(0, CarsPrefab.Length);
            GameObject carPrefab = CarsPrefab[randomIndex];
            Vector3 carPrefabPosition = carPrefab.transform.position;
            Vector3 spawnPos = new(RandomXPosition(), carPrefabPosition.y, zPosition);
            Instantiate(CarsPrefab[randomIndex], spawnPos, CarsPrefab[randomIndex].transform.rotation);
        }

    }
}
