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
        StartCoroutine(PreviewAllCards());
    }

    private IEnumerator PreviewAllCards()
    {
        IsBusy = true;

        Card[] allCards = FindObjectsOfType<Card>();

        // Show all cards (flip front)
        foreach (Card card in allCards)
        {
            card.PreviewFlipFront();
        }

        yield return new WaitForSeconds(1.5f); // Show for a moment

        // Flip back to start the game
        foreach (Card card in allCards)
        {
            card.PreviewFlipBack();
        }

        yield return new WaitForSeconds(1f); // Wait for flip-back to complete

        ResetGame();
        IsBusy = false;
    }

    public void ResetGame()
    {
        firstCard = null;
        secondCard = null;
        IsBusy = false;

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
        yield return new WaitForSeconds(0.6f); // Allow player to view both cards

        if (firstCard.ID == secondCard.ID)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();

            AudioManager.Instance?.PlayMatch();
            ScoreManager.Instance?.AddCorrectMatch();
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();

            AudioManager.Instance?.PlayMismatch();
            ScoreManager.Instance?.AddIncorrectMatch();
        }

        firstCard = null;
        secondCard = null;
        IsBusy = false;

        if (CheckGameOver())
        {
            AudioManager.Instance?.PlayGameOver();
            ScoreManager.Instance?.AddGameCompletionBonus();

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
