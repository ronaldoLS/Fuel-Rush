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

    private TextMeshProUGUI textDistance;
    public Slider sliderFuel;

    [SerializeField] private float difficultyRate = 0.05f;


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
        fuel = 100;
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
}
