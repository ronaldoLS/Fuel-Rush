using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private TextMeshProUGUI textDistance;
    private Slider sliderFuel;
    private GameObject GameOverUI;
    private Image fuelFillImage;
    private Color fuelNormalColor;
    private Button restartButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textDistance = GameObject.Find("Text Distance").GetComponent<TextMeshProUGUI>();
        sliderFuel = GameObject.Find("Fuel Bar").GetComponent<Slider>();

        fuelFillImage = sliderFuel.fillRect.GetComponent<Image>();
        fuelNormalColor = fuelFillImage.color;

        GameOverUI = GameObject.Find("GameOverUI");
        restartButton = GameObject.Find("Restart Button").GetComponent<Button>();
        restartButton.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartGame();
        });
        GameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null)
        {
            textDistance.text = Mathf.FloorToInt(GameManager.Instance.distance) + " m";
            sliderFuel.value = GameManager.Instance.FuelPercent;

            // Low fuel UI warning
            if (GameManager.Instance.FuelPercent < 0.2f)
            {
                fuelFillImage.color =
                    Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time * 4f, 1));
            }
            else
            {
                fuelFillImage.color = fuelNormalColor;
            }

            if (GameManager.Instance.isGameOver)
            {
                GameOverUI.SetActive(true);
            }

        }

    }
}
