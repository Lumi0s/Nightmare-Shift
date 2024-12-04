using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageResizer : MonoBehaviour
{
    [SerializeField] Image      image;
    [SerializeField] Vector2    endSize;
    [SerializeField] float      animationDuration;

    Vector2 startSize;

    void Start()
    {
        startSize = image.rectTransform.sizeDelta;
    }

    private void OnEnable()
    {
        image.rectTransform.DOSizeDelta(endSize, animationDuration);
    }

    void OnDisable()
    {
        image.rectTransform.DOSizeDelta(startSize, animationDuration);
    }
}
