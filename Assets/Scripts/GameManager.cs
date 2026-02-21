using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    int speed = 5;
    float distance = 0;
    float fuel = 100;

    private TextMeshProUGUI textDistance;
    public Slider sliderFuel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    }
    public void increaseFuel(int amount)
    {
        fuel = Mathf.Min(fuel + amount, 100);
    }
}
