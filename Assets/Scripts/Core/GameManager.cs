using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    private const float GameDuration = 180f;
    private const string HighScoreKey = "HighScore";

    private int score;
    private int highScore;
    private float timeRemaining;
    private bool isRunning;

    public int Score => score;
    public int HighScore => highScore;
    public float TimeRemaining => timeRemaining;
    public bool IsRunning => isRunning;
    public bool IsNewHighScore { get; private set; }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UIManager.Instance.ShowStartScreen();
    }

    public void StartGame()
    {
        score = 0;
        timeRemaining = GameDuration;
        IsNewHighScore = false;
        isRunning = true;

        WindowManager.Instance.SpawnAllOrders();
    }

    public void PauseGame()
    {
        isRunning = false;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isRunning = true;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            EndGame();
        }
    }

    public int AddScore(Order order)
    {
        int value = order.CalculateScore(Time.time);
        score += value;
        return value;
    }

    private void EndGame()
    {
        if (score > highScore)
        {
            highScore = score;
            IsNewHighScore = true;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        UIManager.Instance.ShowGameOver(score, IsNewHighScore);
    }
}
