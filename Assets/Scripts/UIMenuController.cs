using UnityEngine;
using UnityEngine.UI;
using TMPro;
[DefaultExecutionOrder(10)]
public class UIMenuController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI recordText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadRecord();
            if (GameManager.Instance.record > 0)
                recordText.text = $"Record: {GameManager.Instance.record:F2} m";
            else
                recordText.text = "";

        }else
        {
            recordText.text = "";
            Debug.LogWarning("GameManager instance not found. Record will not be displayed.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        GameManager.Instance.RestartGame();
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("My Game");
    }
}
