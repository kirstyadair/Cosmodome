using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RectTransform;

/// <summary>
/// Infinitely scrolls an image, provided the screen is not wider than 300% width of the image
/// </summary>
public class HorizontalScrollingBackground : MonoBehaviour
{
    [SerializeField] [Header("Background image to scroll")] Sprite backgroundImage;
    [SerializeField] [Header("Speed to scroll at")]         float scrollSpeed;

    [Space(30)]
    [SerializeField] Image leftImage;
    [SerializeField] Image rightImage;

    [SerializeField] RectTransform scroller;

    RectTransform rectTransform;
    float backgroundWidth;
    float backgroundHeight;

    RectTransform leftRect;
    RectTransform rightRect;

    [SerializeField] float backgroundAndScreenDifference;


    /// <summary>
    /// Swaps out the background image
    /// </summary>
    /// <param name="newBackground"></param>
    public void ChangeBackground(Sprite newBackground) {
        leftImage.sprite = rightImage.sprite = newBackground;
        backgroundImage = newBackground;

        backgroundWidth = newBackground.rect.width;
        backgroundHeight = newBackground.rect.height;

        // set the left image directly to the left of the right
        leftRect = leftImage.GetComponent<RectTransform>();
        leftRect.SetSizeWithCurrentAnchors(Axis.Horizontal, backgroundWidth);
        leftRect.SetSizeWithCurrentAnchors(Axis.Vertical, backgroundHeight);
        leftRect.localPosition = new Vector3(-backgroundWidth, 0);


        rightRect = rightImage.GetComponent<RectTransform>();
        rightRect.SetSizeWithCurrentAnchors(Axis.Horizontal, backgroundWidth);
        rightRect.SetSizeWithCurrentAnchors(Axis.Vertical, backgroundHeight);

        backgroundAndScreenDifference = (backgroundWidth - rectTransform.rect.width) * 0.75f;
        Debug.Log(backgroundAndScreenDifference);
    }

    void Start() {
        leftImage.gameObject.SetActive(true);
        rightImage.gameObject.SetActive(true);

        rectTransform = GetComponent<RectTransform>();
        ChangeBackground(backgroundImage);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move the scroller
        scroller.position += new Vector3(scrollSpeed * Time.deltaTime, 0);

        // Scrolling forwards
        if (scrollSpeed >= 0) {
            if (rightRect.position.x > backgroundAndScreenDifference) rightRect.localPosition -= new Vector3(backgroundWidth * 2, 0);
            if (leftRect.position.x > backgroundAndScreenDifference) leftRect.localPosition -= new Vector3(backgroundWidth * 2, 0);
        } else {
            if (rightRect.position.x < -backgroundAndScreenDifference) rightRect.localPosition += new Vector3(backgroundWidth * 2, 0);
            if (leftRect.position.x < -backgroundAndScreenDifference) leftRect.localPosition += new Vector3(backgroundWidth * 2, 0);
        }
    }
}
