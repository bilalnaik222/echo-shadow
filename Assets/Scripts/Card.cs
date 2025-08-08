using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    public Image frontImage;
    public Image backImage;
    public int cardID;

    private bool isFlipped = false;
    private bool isMatched = false;
    private bool isAnimating = false;

    // Assign card color & ID, reset states
    // public void SetCard(Color frontColor, int id)
    // {
    //     cardID = id;
    //     frontImage.color = frontColor;
    //     frontImage.gameObject.SetActive(false);
    //     backImage.gameObject.SetActive(true);
    //     isFlipped = false;
    //     isMatched = false;
    // }

// assign card sprite & ID, reset states
    public void SetCard(Sprite frontSprite, int id)
    {
        cardID = id;
        frontImage.sprite = frontSprite;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        isFlipped = false;
        isMatched = false;
    }


    // Called by Button onClick
    public void OnClick()
    {
        if (isFlipped || isMatched || CardManager.Instance.IsBusy || isAnimating)
            return;

        StartCoroutine(FlipAnimation());
        CardManager.Instance.OnCardFlipped(this);
        AudioManager.Instance.PlayFlip();
    }

    // Coroutine for flip animation (Y axis rotation)
    private IEnumerator FlipAnimation()
    {
        isAnimating = true;

        // Rotate Y from 0 to 90 degrees
        float time = 0f;
        while (time < 0.25f)
        {
            float angle = Mathf.Lerp(0, 90, time / 0.25f);
            transform.localRotation = Quaternion.Euler(0, angle, 0);
            time += Time.deltaTime;
            yield return null;
        }

        // Toggle front/back visibility mid-flip
        isFlipped = !isFlipped;
        frontImage.gameObject.SetActive(isFlipped);
        backImage.gameObject.SetActive(!isFlipped);

        // Rotate Y from 90 to 0 degrees
        time = 0f;
        while (time < 0.25f)
        {
            float angle = Mathf.Lerp(90, 0, time / 0.25f);
            transform.localRotation = Quaternion.Euler(0, angle, 0);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        isAnimating = false;
    }

    // Called by CardManager if mismatch to flip back
    public void FlipBack()
    {
        if (isAnimating) return;

        StartCoroutine(FlipBackAnimation());
    }

    private IEnumerator FlipBackAnimation()
    {
        isAnimating = true;

        // Rotate Y from 0 to 90 degrees
        float time = 0f;
        while (time < 0.25f)
        {
            float angle = Mathf.Lerp(0, 90, time / 0.25f);
            transform.localRotation = Quaternion.Euler(0, angle, 0);
            time += Time.deltaTime;
            yield return null;
        }

        // Set back side visible, front hidden
        isFlipped = false;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);

        // Rotate Y from 90 to 0 degrees
        time = 0f;
        while (time < 0.25f)
        {
            float angle = Mathf.Lerp(90, 0, time / 0.25f);
            transform.localRotation = Quaternion.Euler(0, angle, 0);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        isAnimating = false;
    }

    // Mark card as matched, disable interaction optionally
    public void SetMatched()
    {
        isMatched = true;
        // Optional: disable Button or add visual effect
    }

    // Public getters
    public int ID => cardID;
    public bool IsMatched => isMatched;
}
