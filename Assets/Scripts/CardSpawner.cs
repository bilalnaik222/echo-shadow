using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardContainer;

    [Header("Grid Settings")]
    public int rows = 3;
    public int columns = 3;

    [Header("Grid Spacing")]
    public Vector2 spacing = new Vector2(10f, 10f);

    [Header("Card Front Sprites")]
    public List<Sprite> cardFrontSprites = new List<Sprite>();

    void Start()
    {
        ClearOldCards();
        SetupGridLayout();
        GenerateCards();
    }

    void SetupGridLayout()
    {
        GridLayoutGroup grid = cardContainer.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            Debug.LogError("GridLayoutGroup component is missing on cardContainer.");
            return;
        }

        grid.cellSize = new Vector2(130f, 130f);
        grid.spacing = spacing;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        Debug.Log("Grid cell size set to fixed 130x130.");
    }

    void GenerateCards()
    {
        int totalCards = rows * columns;

        if (totalCards % 2 != 0)
        {
            Debug.LogWarning("Odd number of cards: one card will not have a pair.");
        }

        int pairCount = totalCards / 2;
        List<int> cardIDs = new List<int>();

        for (int i = 0; i < pairCount; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        if (totalCards % 2 != 0 && pairCount > 0)
        {
            cardIDs.Add(Random.Range(0, pairCount));
        }

        Shuffle(cardIDs);

        foreach (int id in cardIDs)
        {
            if (id >= cardFrontSprites.Count)
            {
                Debug.LogError($"Not enough unique sprites in cardFrontSprites list for card ID {id}.");
                continue;
            }

            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            Card card = cardObj.GetComponent<Card>();
            card.SetCard(cardFrontSprites[id], id);

            Button button = cardObj.GetComponent<Button>();
            if (button == null)
                button = cardObj.AddComponent<Button>();

            // Capture local reference for correct closure in listener
            Card localCard = card;
            button.onClick.AddListener(() => localCard.OnClick());
        }
    }

    void ClearOldCards()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
