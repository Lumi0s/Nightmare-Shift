using System;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceItemsActivator : MonoBehaviour
{
    [SerializeField] List<GameObject> items;

    public bool IsCurrentlyActivated => isCurrentlyActivated;
    public Action<bool> onActivationStateChange;

    bool isCurrentlyActivated = false;

    public void SetActivationState(bool activate)
    {
        if (isCurrentlyActivated == activate)
        {
            return;
        }

        isCurrentlyActivated = activate;

        foreach (GameObject item in items)
        {
            item.SetActive(activate);
        }

        onActivationStateChange?.Invoke(activate);
    }
}
