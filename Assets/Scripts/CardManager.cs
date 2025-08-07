using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    private Card firstCard = null;
    private Card secondCard = null;

    public bool IsBusy { get; private set; } = false;

    public int score = 0;
    private int comboCount = 0;

    // Set your UI manager reference to update UI (optional)
    // public UIManager uiManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadProgress();
    }

    public void OnCardFlipped(Card card)
    {
        if (IsBusy) return;

        if (firstCard == null)
        {
            firstCard = card;
            // Optionally play flip sound here (already called in Card.OnClick)
        }
        else if (secondCard == null && card != firstCard)
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        IsBusy = true;

        yield return new WaitForSeconds(0.6f); // Wait to let player see second flipped card

        if (firstCard.ID == secondCard.ID)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();

            comboCount++;
            int pointsEarned = 10 * comboCount; // combo multiplier example
            score += pointsEarned;

            AudioManager.Instance.PlayMatch();

            // Optionally update UI here
            // uiManager.UpdateScore(score);
            // uiManager.ShowCombo(comboCount);

            // Save progress after match
            SaveProgress();

            CheckGameOver();
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();

            comboCount = 0;
            AudioManager.Instance.PlayMismatch();
        }

        firstCard = null;
        secondCard = null;
        IsBusy = false;
    }

    private void CheckGameOver()
    {
        Card[] allCards = FindObjectsOfType<Card>();
        foreach (var card in allCards)
        {
            if (!card.IsMatched)
                return; // Game not over yet
        }

        // All cards matched — game over!
        AudioManager.Instance.PlayGameOver();

        // Optionally show game over UI or restart prompt
        Debug.Log("Game Over! Final Score: " + score);
    }

    public void SaveProgress()
    {
        Card[] allCards = FindObjectsOfType<Card>();
        List<int> matchedIds = new List<int>();
        foreach (var card in allCards)
        {
            if (card.IsMatched)
                matchedIds.Add(card.ID);
        }
        string matchedStr = string.Join(",", matchedIds);
        PlayerPrefs.SetString("matchedCards", matchedStr);

        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        string matchedStr = PlayerPrefs.GetString("matchedCards", "");
        score = PlayerPrefs.GetInt("score", 0);

        if (!string.IsNullOrEmpty(matchedStr))
        {
            string[] parts = matchedStr.Split(',');
            List<int> matchedIds = new List<int>();
            foreach (string p in parts)
            {
                if (int.TryParse(p, out int id))
                    matchedIds.Add(id);
            }

            Card[] allCards = FindObjectsOfType<Card>();
            foreach (var card in allCards)
            {
                if (matchedIds.Contains(card.ID))
                    card.SetMatched();
            }
        }

        // Optionally update UI here
        // uiManager.UpdateScore(score);
    }

    // Optional: clear saved data for testing
    public void ClearSave()
    {
        PlayerPrefs.DeleteKey("matchedCards");
        PlayerPrefs.DeleteKey("score");
        PlayerPrefs.Save();
    }
}
