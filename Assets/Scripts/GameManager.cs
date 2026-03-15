using UnityEngine;
using System.IO;
[DefaultExecutionOrder(1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isGameOver;
    public float speed;
    public float baseSpeed { get; private set; }
    public float maxSpeed { get; private set; }
    public float distance { get; private set; }
    public float fuel { get; private set; }
    public float maxFuel { get; private set; }
    public float record { get; private set; }


    [SerializeField] private float lowFuelThreshold = 0.15f;
    [SerializeField] private float stutterStrength = 0.25f;
    [SerializeField] private float stutterSpeed = 8f;

    public GameObject GameOverUI;
    public float FuelPercent => fuel / maxFuel;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        record = LoadRecord();
        speed = 0;
        baseSpeed = 5;
        maxSpeed = 25;
        distance = 0;
        maxFuel = 100;
        fuel = maxFuel;
    }

    private void Start()
    {
        isGameOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        if (FuelPercent <= 0.01)
        {
            GameOver();
            return;
        }
        // Difficulty progression
        float difficultyT = Mathf.Clamp01(distance / 500f);
        float difficultySpeed = Mathf.Lerp(baseSpeed, maxSpeed, difficultyT);

        // Fuel penalty (starts at 25%)
        float fuelMultiplier = Mathf.Clamp01(FuelPercent / 0.25f);


        // Engine stutter effect when fuel is low
        if (FuelPercent <= lowFuelThreshold && FuelPercent > 0)
        {
            float stutter = Mathf.Sin(Time.time * stutterSpeed) * stutterStrength;

            fuelMultiplier *= (1f - Mathf.Abs(stutter));
        }

        speed = difficultySpeed * fuelMultiplier;

        // Distance
        distance += speed * Time.deltaTime;

        // Fuel consumption
        fuel -= (speed * Time.deltaTime) * 0.4f;

    }
    public void IncreaseFuel(int amount)
    {
        fuel = Mathf.Min(fuel + amount, 100);
    }
    public void GameOver()
    {
        if (distance > record)
        {
            record = distance;
            SaveRecord();
        }
        isGameOver = true;
        speed = 0;
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        isGameOver = false;
        speed = 0;
        distance = 0;
        fuel = maxFuel;
        Time.timeScale = 1f;
    }
   
    public void SaveRecord()
    {
        SaveData data = new SaveData();
        data.highScore = distance;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    }
    public float LoadRecord()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            record = data.highScore;
            return record;
        }
        return 0;
    }
    public void CleanRecord()
    {
        distance = 0;
        SaveRecord();
    }
}
