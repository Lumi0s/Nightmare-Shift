using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    void Update()
    {
        if (isHolding)
        {
            PlaceholderWinningSystem.Instance.IncreaseProgress();
        }
    }
}

