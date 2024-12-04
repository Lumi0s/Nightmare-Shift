using DG.Tweening;
using System.Text;
using TMPro;
using UnityEngine;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] Color positiveColor;
    [SerializeField] Color negativeColor;

    const string positiveIndicator = "+";
    const string negativeIndicator = "-";
    const float animDuration = 0.5f;

    void Start()
    {
        rectTransform.DOScale(0, 0);
    }

    public void UpdateScoreIndicator(float score)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(score > 0 ? positiveIndicator : negativeIndicator);
        sb.Append(Mathf.Abs(score));
        textDisplay.text = sb.ToString();
        textDisplay.color = score > 0 ? positiveColor : negativeColor;
        rectTransform.DOScale(1, animDuration).OnComplete(() => rectTransform.DOScale(0, animDuration));
    }
}
