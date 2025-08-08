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

    // Assign card sprite & ID, reset states
    public void SetCard(Sprite frontSprite, int id)
    {
        cardID = id;
        frontImage.sprite = frontSprite;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        isFlipped = false;
        isMatched = false;
        transform.localRotation = Quaternion.identity;
    }

    // Called by Button onClick
    public void OnClick()
    {
        if (isFlipped || isMatched || CardManager.Instance.IsBusy || isAnimating)
            return;

        StartCoroutine(FlipAnimation(true));
        CardManager.Instance.OnCardFlipped(this);
        AudioManager.Instance?.PlayFlip();
    }

    // Flip animation used for both gameplay & preview
    private IEnumerator FlipAnimation(bool flipToFront)
    {
        isAnimating = true;

        float time = 0f;
        while (time < 0.25f)
        {
            float angle = Mathf.Lerp(0, 90, time / 0.25f);
            transform.localRotation = Quaternion.Euler(0, angle, 0);
            time += Time.deltaTime;
            yield return null;
        }

        if (flipToFront)
        {
            isFlipped = true;
            frontImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(false);
        }
        else
        {
            isFlipped = false;
            frontImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);
        }

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

    public void FlipBack()
    {
        if (isAnimating) return;
        StartCoroutine(FlipAnimation(false));
    }

    // ✅ PREVIEW: Flip card to front with animation & sound
    public void PreviewFlipFront()
    {
        StartCoroutine(FlipAnimation(true));
        AudioManager.Instance?.PlayFlip();
    }

    // ✅ PREVIEW: Flip card to back with animation & sound
    public void PreviewFlipBack()
    {
        StartCoroutine(FlipAnimation(false));
        AudioManager.Instance?.PlayFlip();
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public int ID => cardID;
    public bool IsMatched => isMatched;
}
