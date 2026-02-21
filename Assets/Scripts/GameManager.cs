using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int speed { get; private set; }
    public float distance { get; private set; }
    public float fuel { get; private set; }

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
        speed = 5;
        distance = 0;
        fuel = 100;
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
