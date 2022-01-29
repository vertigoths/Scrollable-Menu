using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollRectListener : MonoBehaviour
{
    [SerializeField] private ButtonsTabController BTC;
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private RectTransform contentPanel;
    [SerializeField] private RectTransform colorBar;
    [SerializeField] private int[] values;
    [SerializeField] private Button[] buttons;
    [SerializeField] private int[] colorBarPositions;

    private int dragBeginPosition;

    public void OnDrag()
    {
        contentPanel.anchoredPosition = new Vector2(Mathf.Clamp(contentPanel.anchoredPosition.x, dragBeginPosition - 800f, dragBeginPosition + 800f), contentPanel.anchoredPosition.y);
        contentPanel.anchoredPosition = new Vector2(Mathf.Clamp(contentPanel.anchoredPosition.x, -2500f, 2500f), contentPanel.anchoredPosition.y);
        colorBar.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x / -7f, 0f);
    }

    public void BeginDrag()
    {
        dragBeginPosition = (int)contentPanel.anchoredPosition.x;
    }

	public void EndDrag()
	{
		StartCoroutine(SnapToThePosition(GetTheClosestScreensPosition((int)contentPanel.anchoredPosition.x)));
	}

    public Button SelectButtonToGrow()
    {
        int positionX = (int)contentPanel.anchoredPosition.x;

        for(int i = 0; i < values.Length; i++)
        {
            if (values[i] == positionX)
            {
                return buttons[i];
            }
        }

        return buttons[2];
    }

	public IEnumerator SnapToThePosition(int toWhere)
	{
		float elapsedTime = 0;
		Vector2 startingPosition = contentPanel.anchoredPosition;
		Vector2 endingPosition = new Vector2(toWhere, 0);

        Vector2 barStartingPosition = colorBar.anchoredPosition;
        Vector2 barEndingPosition = new Vector2(BarToWhere(toWhere), 0);

		while (elapsedTime < 0.15f)
		{
			contentPanel.anchoredPosition = Vector2.Lerp(startingPosition, endingPosition, elapsedTime / 0.15f);
            colorBar.anchoredPosition = Vector2.Lerp(barStartingPosition, barEndingPosition, elapsedTime / 0.15f);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		contentPanel.anchoredPosition = endingPosition;
        colorBar.anchoredPosition = barEndingPosition;
        BTC.ChangeWidthOfButton(SelectButtonToGrow());
    }

	public int GetTheClosestScreensPosition(int currentPosition)
	{
		int closestScreenPosition = 0;
		int difference = int.MaxValue;

		for(int i = 0; i < values.Length; i++)
		{
			if (Mathf.Abs(values[i] - currentPosition) <= difference)
			{
				difference = (Mathf.Abs(values[i] - currentPosition));
				closestScreenPosition = i;
			}
		}

		return values[closestScreenPosition];
	}

    public int BarToWhere(int toWhere)
    {
        for(int i = 0; i < values.Length; i++)
        {
            if(toWhere == values[i])
            {
                return colorBarPositions[i];
            }
        }

        return 0;
    }
}
