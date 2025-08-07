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

    [Header("Colors for Front Sides")]
    public List<Color> cardColors = new List<Color>
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        Color.gray,
        Color.white,
        Color.black
    };

    void Start()
    {
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

        // ✅ Fixed size: 130 x 130
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

        // Generate pairs
        for (int i = 0; i < pairCount; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        // Add unmatched card if odd number of cards
        if (totalCards % 2 != 0)
        {
            cardIDs.Add(Random.Range(0, pairCount));
        }

        Shuffle(cardIDs);

        foreach (int id in cardIDs)
        {
            if (id >= cardColors.Count)
            {
                Debug.LogError($"Not enough unique colors in cardColors list for card ID {id}.");
                continue;
            }

            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            Card card = cardObj.GetComponent<Card>();
            card.SetCard(cardColors[id], id);

            Button button = cardObj.GetComponent<Button>();
            if (button == null)
                button = cardObj.AddComponent<Button>();

            button.onClick.AddListener(() => card.OnClick());
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
