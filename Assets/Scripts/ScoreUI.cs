using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text scoreText;
    public Text comboText;

    void Update()
    {
        if (ScoreManager.Instance == null) return;

        scoreText.text = "Score: " + ScoreManager.Instance.Score;
        comboText.text = "Combo: " + ScoreManager.Instance.ComboCount;
    }
}
