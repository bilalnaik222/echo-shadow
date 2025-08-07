using System.Collections;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    private Card firstCard = null;
    private Card secondCard = null;

    public bool IsBusy { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnCardFlipped(Card card)
    {
        if (IsBusy) return;

        if (firstCard == null)
        {
            firstCard = card;
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

        yield return new WaitForSeconds(0.6f); // Allow player to see flipped cards

        if (firstCard.ID == secondCard.ID)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
            AudioManager.Instance.PlayMatch();
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
            AudioManager.Instance.PlayMismatch();
        }

        if (CheckGameOver())
        {
            AudioManager.Instance.PlayGameOver();
            // Add game over logic here if needed
        }

        firstCard = null;
        secondCard = null;
        IsBusy = false;
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
