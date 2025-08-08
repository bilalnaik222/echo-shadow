using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    private Card firstCard = null;
    private Card secondCard = null;

    public bool IsBusy { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetGame(); // Ensure clean state
    }

    public void ResetGame()
    {
        firstCard = null;
        secondCard = null;
        IsBusy = false;

        // Reset score when game starts
        ScoreManager.Instance?.ResetScore();
    }

    public void OnCardFlipped(Card card)
    {
        if (IsBusy || card == firstCard || card.IsMatched)
            return;

        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        IsBusy = true;
        yield return new WaitForSeconds(0.6f); // Let player see cards

        if (firstCard.ID == secondCard.ID)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();

            AudioManager.Instance?.PlayMatch();
            ScoreManager.Instance?.AddCorrectMatch(); // ✅ +100 + combo bonus
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();

            AudioManager.Instance?.PlayMismatch();
            ScoreManager.Instance?.AddIncorrectMatch(); // ✅ -10, reset combo
        }

        firstCard = null;
        secondCard = null;
        IsBusy = false;

        if (CheckGameOver())
        {
            AudioManager.Instance?.PlayGameOver();
            ScoreManager.Instance?.AddGameCompletionBonus(); // ✅ +500

            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }
    }

    private bool CheckGameOver()
    {
        Card[] allCards = FindObjectsOfType<Card>();
        foreach (Card c in allCards)
        {
            if (!c.IsMatched)
                return false;
        }
        return true;
    }
}
