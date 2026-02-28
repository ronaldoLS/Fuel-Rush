using TMPro;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float speed { get; private set; }
    [SerializeField] private float baseSpeed = 5f;
    public float maxSpeed { get; private set; }
    public float distance { get; private set; }
    public float fuel { get; private set; }

    private TextMeshProUGUI textDistance;
    public Slider sliderFuel;
    
    [SerializeField] private float difficultyRate = 0.05f;
    public float DifficultyPercent =>
    Mathf.InverseLerp(baseSpeed, maxSpeed, speed);

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        speed = 5;
        distance = 0;
        fuel = 100;
        baseSpeed = 5;
        maxSpeed = 40;
    }

    private void Start()
    {
        textDistance = GameObject.Find("Text Distance").GetComponent<TextMeshProUGUI>();
        sliderFuel = GameObject.Find("Fuel Bar").GetComponent<Slider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        distance += speed * Time.deltaTime;
        textDistance.text = Mathf.FloorToInt(distance) + " m";
        fuel -= Time.deltaTime;
        sliderFuel.value = fuel / 100;

        float t = distance / 500f; // quanto maior, mais lento cresce
        float difficultyMultiplier = Mathf.Lerp(1f, 2.5f, t);

        speed = Mathf.Clamp(baseSpeed * difficultyMultiplier, baseSpeed, maxSpeed);

    }
    public void increaseFuel(int amount)
    {
        fuel = Mathf.Min(fuel + amount, 100);
    }
}
