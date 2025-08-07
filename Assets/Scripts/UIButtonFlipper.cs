using System.Collections;
using UnityEngine;

public class UIButtonFlipper : MonoBehaviour
{
    public float flipInterval = 2f;
    public float flipDuration = 0.5f;

    public GameObject frontSide;
    public GameObject backSide;
    public AudioClip flipSound;

    private RectTransform rectTransform;
    private bool isFlipping = false;
    private bool isFrontVisible = true;

    private AudioSource audioSource;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = flipSound;
        audioSource.volume = 0.7f;

        InvokeRepeating(nameof(TriggerFlip), flipInterval, flipInterval);
    }

    void TriggerFlip()
    {
        if (!isFlipping)
            StartCoroutine(Flip());
    }

    private IEnumerator Flip()
    {
        isFlipping = true;

        if (flipSound != null)
            audioSource.Play();

        float time = 0f;
        float startY = rectTransform.localRotation.eulerAngles.y;
        float endY = startY + 180f;

        while (time < flipDuration / 2f)
        {
            float y = Mathf.Lerp(startY, startY + 90f, time / (flipDuration / 2f));
            rectTransform.localRotation = Quaternion.Euler(0, y, 0);
            time += Time.deltaTime;
            yield return null;
        }

        // Toggle front/back visibility
        isFrontVisible = !isFrontVisible;
        frontSide.SetActive(isFrontVisible);
        backSide.SetActive(!isFrontVisible);

        time = 0f;
        while (time < flipDuration / 2f)
        {
            float y = Mathf.Lerp(startY + 90f, endY, time / (flipDuration / 2f));
            rectTransform.localRotation = Quaternion.Euler(0, y, 0);
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.localRotation = Quaternion.Euler(0, endY % 360f, 0);
        isFlipping = false;
    }
}
