using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image frontImage;
    public Image backImage;
    public int cardID;

    private bool isFlipped = false;

    public void SetCard(Color frontColor, int id)
    {
        cardID = id;
        frontImage.color = frontColor;
        frontImage.gameObject.SetActive(false); // hidden at start
        backImage.gameObject.SetActive(true);
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        frontImage.gameObject.SetActive(isFlipped);
        backImage.gameObject.SetActive(!isFlipped);
    }

    public void OnClick()
    {
        Flip();
        // Add call to GameManager here if needed
    }
}
