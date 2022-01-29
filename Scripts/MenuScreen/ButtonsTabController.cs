using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonsTabController : MonoBehaviour
{
	[SerializeField] private Button previousButton;
    [SerializeField] private ScrollRectListener SRL;

    [SerializeField] private float buttonsTabAnimationSpeed;

    public void OnClick(Button button)
	{
        ChangeWidthOfButton(button);
        StartCoroutine(SRL.SnapToThePosition(ToWhere(button.name)));
    }

    public void ChangeWidthOfButton(Button button)
    {
        StartCoroutine(FadeOffAndMoveImage(previousButton, button));
        previousButton.GetComponent<LayoutElement>().preferredWidth = -1;
        button.GetComponent<LayoutElement>().preferredWidth = 180;
        previousButton = button;
    }

    public IEnumerator FadeOffAndMoveImage(Button previousButton, Button currentButton)
    {
        float elapsedTime = 0;
        RectTransform currentButtonImage = previousButton.transform.GetChild(1).GetComponent<RectTransform>();
        Vector2 currentButtonStartingPosition = currentButtonImage.anchoredPosition;
        Vector2 currentButtonEndingPositon = new Vector2(0f, 0f);

        Text currentText = previousButton.transform.GetChild(0).GetComponent<Text>();

        Vector3 currentButtonImageScaleBeginning = currentButtonImage.localScale;
        Vector3 currentButtonImageScaleEnding = new Vector3(1f, 1f, 1f);

        RectTransform nextButtonImage = currentButton.transform.GetChild(1).GetComponent<RectTransform>();
        Vector2 nextButtonStartingPosition = nextButtonImage.anchoredPosition;
        Vector2 nextButtonEndingPosition = new Vector2(0f, 35f);

        Text nextText = currentButton.transform.GetChild(0).GetComponent<Text>();

        float textColorStartPosition = 0f;
        float textColorEndPosition = 1f;

        Vector3 nextButtonImageScaleBeginning = nextButtonImage.localScale;
        Vector3 nextButtonImageScaleEnding = new Vector3(1.4f, 1.3f, 1f);

        while (elapsedTime < buttonsTabAnimationSpeed)
        {
            currentButtonImage.anchoredPosition = Vector2.Lerp(currentButtonStartingPosition, currentButtonEndingPositon, elapsedTime / buttonsTabAnimationSpeed);
            nextButtonImage.anchoredPosition = Vector2.Lerp(nextButtonStartingPosition, nextButtonEndingPosition, elapsedTime / buttonsTabAnimationSpeed);

            currentButtonImage.localScale = Vector2.Lerp(currentButtonImageScaleBeginning, currentButtonImageScaleEnding, elapsedTime / buttonsTabAnimationSpeed);
            nextButtonImage.localScale = Vector2.Lerp(nextButtonImageScaleBeginning, nextButtonImageScaleEnding, elapsedTime / buttonsTabAnimationSpeed);

            currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, currentText.color.a - (Time.deltaTime * (1f / buttonsTabAnimationSpeed)));
            nextText.color = new Color(nextText.color.r, nextText.color.g, nextText.color.b, nextText.color.a + (Time.deltaTime * (1f / buttonsTabAnimationSpeed)));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        currentButtonImage.anchoredPosition = currentButtonEndingPositon;
        nextButtonImage.anchoredPosition = nextButtonEndingPosition;

        currentButtonImage.localScale = currentButtonImageScaleEnding;
        nextButtonImage.localScale = nextButtonImageScaleEnding;

        currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, textColorStartPosition);
        nextText.color = new Color(nextText.color.r, nextText.color.g, nextText.color.b, textColorEndPosition);
    }

    public int ToWhere(string buttonName)
    {
        if (buttonName.Equals("ShopButton"))
        {
            return 2160;
        }

        else if (buttonName.Equals("CardsButton"))
        {
            return 1080;
        }

        else if (buttonName.Equals("WarButton"))
        {
            return 0;
        }

        else if (buttonName.Equals("ClanButton"))
        {
            return -1080;
        }

        return -2160;
    }
}
