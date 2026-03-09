using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float speed;
    public float baseSpeed { get; private set; }
    public float maxSpeed { get; private set; }
    public float distance { get; private set; }
    public float fuel { get; private set; }

<<<<<<< Updated upstream
    private TextMeshProUGUI textDistance;
    public Slider sliderFuel;

    [SerializeField] private float difficultyRate = 0.05f;
=======
    [SerializeField] private float lowFuelThreshold = 0.15f;
    [SerializeField] private float stutterStrength = 0.25f;
    [SerializeField] private float stutterSpeed = 8f;

    public float FuelPercent => fuel / maxFuel;

>>>>>>> Stashed changes


    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        speed = 0;
        baseSpeed = 5;
        maxSpeed = 25;
        distance = 0;
        fuel = 100;
    }

    private void Start()
    {
<<<<<<< Updated upstream
        textDistance = GameObject.Find("Text Distance").GetComponent<TextMeshProUGUI>();
        sliderFuel = GameObject.Find("Fuel Bar").GetComponent<Slider>();
=======
>>>>>>> Stashed changes

    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        distance += speed * Time.deltaTime;
        textDistance.text = Mathf.FloorToInt(distance) + " m";
        fuel -= (speed * Time.deltaTime) / 2;
        if (fuel <= 0)
        {
            fuel = 0;
            //speed = 0;
            // Aqui você pode adicionar lógica para lidar com o fim do jogo, como mostrar uma tela de game over
        }
        sliderFuel.value = fuel / 100;

        float t = distance / 500f; // quanto maior, mais lento cresce
        float difficultyMultiplier = Mathf.Lerp(1f, 5f, t);

        speed = Mathf.Clamp(baseSpeed * difficultyMultiplier, baseSpeed, maxSpeed);

    }
    public void increaseFuel(int amount)
    {
        fuel = Mathf.Min(fuel + amount, 100);
    }
=======
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f; // Garanta que o tempo volte ao normal ao carregar
    }

    public void IncreaseFuel(int amount)
    {
        fuel = Mathf.Min(fuel + amount, 100);
    }
    public void GameOver()
    {
        isGameOver = true;
        speed = 0;
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        speed = 0;
        distance = 0;
        fuel = maxFuel;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
>>>>>>> Stashed changes
}
