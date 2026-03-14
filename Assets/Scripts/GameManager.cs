using TMPro;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isGameOver = false;
    public float speed;
    public float baseSpeed { get; private set; }
    public float maxSpeed { get; private set; }
    public float distance { get; private set; }
    public float fuel { get; private set; }
    public float maxFuel { get; private set; }

    private TextMeshProUGUI textDistance;
    public Slider sliderFuel;

    [SerializeField] private float lowFuelThreshold = 0.15f;
    [SerializeField] private float stutterStrength = 0.25f;
    [SerializeField] private float stutterSpeed = 8f;

    public GameObject GameOverUI;
    public float FuelPercent => fuel / maxFuel;
    private Image fuelFillImage;
    private Color fuelNormalColor;


    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        speed = 0;
        baseSpeed = 5;
        maxSpeed = 25;
        distance = 0;
        maxFuel = 100;
        fuel = maxFuel;
    }

    private void Start()
    {
        textDistance = GameObject.Find("Text Distance").GetComponent<TextMeshProUGUI>();
        sliderFuel = GameObject.Find("Fuel Bar").GetComponent<Slider>();

        fuelFillImage = sliderFuel.fillRect.GetComponent<Image>();
        fuelNormalColor = fuelFillImage.color;

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
        textDistance.text = Mathf.FloorToInt(distance) + " m";

        // Fuel consumption
        fuel -= (speed * Time.deltaTime) * 0.4f;
        sliderFuel.value = FuelPercent;

        // Low fuel UI warning
        if (FuelPercent < 0.2f)
        {
            fuelFillImage.color =
                Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time * 4f, 1));
        }
        else
        {
            fuelFillImage.color = fuelNormalColor;
        }

    }
    public void IncreaseFuel(int amount)
    {
        fuel = Mathf.Min(fuel + amount, 100);
    }
    public void GameOver()
    {
        isGameOver = true;
        speed = 0;
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
