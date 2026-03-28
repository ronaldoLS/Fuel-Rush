using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDistance;
    [SerializeField] private Slider sliderFuel;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button restartButton;
    private Image fuelFillImage;
    private Color fuelNormalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textDistance = GameObject.Find("Text Distance").GetComponent<TextMeshProUGUI>();
        sliderFuel = GameObject.Find("Fuel Bar").GetComponent<Slider>();
        gameOverUI = GameObject.Find("GameOverUI");
        restartButton = GameObject.Find("Restart Button").GetComponent<Button>();
        restartButton.onClick.AddListener(RestartGame);

        fuelFillImage = sliderFuel.fillRect.GetComponent<Image>();
        fuelNormalColor = fuelFillImage.color;

        gameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        textDistance.text = Mathf.FloorToInt(GameManager.Instance.distance) + " m";

        sliderFuel.value = GameManager.Instance.FuelPercent;

        if (GameManager.Instance.isGameOver)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            gameOverUI.SetActive(false);
        }
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
    }
    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMenu()
    {
        AudioManager.Instance.musicSource.Stop();
        SceneManager.LoadScene("MainMenu");
    }
}
