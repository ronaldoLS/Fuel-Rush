using TMPro;
using UnityEngine;
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
    public float maxFuel { get; private set; }

    private TextMeshProUGUI textDistance;
    public Slider sliderFuel;

    [SerializeField] private float difficultyRate = 0.05f;
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

    }

    // Update is called once per frame
    void Update()
    {
        if (fuel <= 0)
        {
            speed = 0;
            return;
        }

        // Difficulty progression
        float difficultyT = Mathf.Clamp01(distance / 500f);
        float difficultySpeed = Mathf.Lerp(baseSpeed, maxSpeed, difficultyT);

        // Fuel penalty (starts at 25%)
        float fuelMultiplier = Mathf.Clamp01(FuelPercent / 0.25f);

        speed = difficultySpeed * fuelMultiplier;

        // Distance
        distance += speed * Time.deltaTime;
        textDistance.text = Mathf.FloorToInt(distance) + " m";

        // Fuel consumption
        fuel -= (speed * Time.deltaTime) * 0.4f;
        sliderFuel.value = FuelPercent;

    }
    public void IncreaseFuel(int amount)
    {
        fuel = Mathf.Min(fuel + amount, 100);
    }
}
