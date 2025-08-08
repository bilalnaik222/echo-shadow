using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score { get; private set; }
    public int ComboCount { get; private set; }
    public int HighScore { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCorrectMatch()
    {
        ComboCount++;
        int comboBonus = (ComboCount - 1) * 50;
        Score += 100 + comboBonus;
        SaveScore();
    }

    public void AddIncorrectMatch()
    {
        ComboCount = 0;
        Score -= 10;
        if (Score < 0) Score = 0;
        SaveScore();
    }

    public void AddGameCompletionBonus()
    {
        Score += 500;

        if (Score > HighScore)
        {
            HighScore = Score;
        }

        SaveScore();
    }

    public void ResetScore()
    {
        Score = 0;
        ComboCount = 0;
        SaveScore();
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.SetInt("ComboCount", ComboCount);
        PlayerPrefs.SetInt("HighScore", HighScore);
        PlayerPrefs.Save();
    }

    private void LoadScore()
    {
        Score = PlayerPrefs.GetInt("Score", 0);
        ComboCount = PlayerPrefs.GetInt("ComboCount", 0);
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void ResetHighScore()
    {
        HighScore = 0;
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
    }
}
