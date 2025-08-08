using UnityEngine;
using UnityEngine.UI;

public class MainMenuScoreDisplay : MonoBehaviour
{
    public Text highScoreText;
    public Text lastScoreText;

    void Start()
    {
        if (ScoreManager.Instance == null) return;

        highScoreText.text = "High Score: " + ScoreManager.Instance.HighScore;
        lastScoreText.text = "Last Score: " + ScoreManager.Instance.Score;
    }
}
