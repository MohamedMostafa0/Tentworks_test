using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : BaseSingleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject interactionPanel;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI heldItemText;

    [Header("Interaction Panel")]
    [SerializeField] private TextMeshProUGUI interactionTitleText;
    [SerializeField] private Button okButton;
    [SerializeField] private TextMeshProUGUI okButtonText;

    [Header("Game Over Panel")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject newHighScoreObject;

    // ─── Screens ─────────────────────────────────────────────────────────────

    public void ShowStartScreen()
    {
        startPanel.SetActive(true);
        hudPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        interactionPanel.SetActive(false);
    }

    public void ShowGameOver(int score, bool isNewHighScore)
    {
        gameOverPanel.SetActive(true);
        hudPanel.SetActive(false);
        interactionPanel.SetActive(false);
        finalScoreText.text = $"Score: {score}";
        newHighScoreObject.SetActive(isNewHighScore);
    }

    // ─── Button callbacks (wired in Inspector) ────────────────────────────────

    public void OnPlayPressed()
    {
        startPanel.SetActive(false);
        hudPanel.SetActive(true);
        GameManager.Instance.StartGame();
    }

    public void OnPausePressed()
    {
        pausePanel.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public void OnResumePressed()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    public void OnRestartPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ─── Interaction panel ────────────────────────────────────────────────────

    public void ShowInteractionUI(InteractionUIData data)
    {
        interactionPanel.SetActive(true);
        interactionTitleText.text = data.Title;
        okButton.gameObject.SetActive(data.ShowOkButton);

        if (data.ShowOkButton)
        {
            okButtonText.text = data.OkLabel;
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(() =>
            {
                data.OnOk?.Invoke();
                HideInteractionUI();
            });
        }
    }

    public void HideInteractionUI()
    {
        interactionPanel.SetActive(false);
    }

    // ─── HUD update ───────────────────────────────────────────────────────────

    private void Update()
    {
        if (!GameManager.Instance.IsRunning) return;

        scoreText.text = $"Score: {GameManager.Instance.Score}";
        highScoreText.text = $"Best: {GameManager.Instance.HighScore}";
        timerText.text = FormatTime(GameManager.Instance.TimeRemaining);

        if (heldItemText != null)
        {
            if (PlayerController.Instance.HasItem())
            {
                Ingredient item = PlayerController.Instance.GetItem();
                heldItemText.text = $"Holding: {item.Type} ({item.State})";
            }
            else
            {
                heldItemText.text = "Holding: Nothing";
            }
        }
    }

    private static string FormatTime(float seconds)
    {
        int m = Mathf.FloorToInt(seconds / 60f);
        int s = Mathf.FloorToInt(seconds % 60f);
        return $"{m}:{s:00}";
    }
}
