using TMPro;
using UnityEngine;

public class InputFieldActivator : MonoBehaviour
{
    [SerializeField] TMP_InputField                 inputField;
    [SerializeField] UserInterfaceItemsActivator    itemActivator;
    [SerializeField] bool                           clearOnStateChange = false;

    public bool IsActivated => isActivated;
    bool isActivated = false;

    void Start()
    {
        itemActivator.onActivationStateChange += OnActivationStateChange;
    }

    void OnActivationStateChange(bool activated)
    {
        isActivated = activated;
        inputField.interactable = activated;

        if (clearOnStateChange)
        {
            inputField.text = string.Empty;
        }

        if (activated)
        {
            inputField.ActivateInputField();
        }
        else
        {
            inputField.DeactivateInputField();
        }

    }
}
