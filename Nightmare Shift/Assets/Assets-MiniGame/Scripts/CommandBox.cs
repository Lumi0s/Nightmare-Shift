using TMPro;
using UnityEngine;

public class CommandBox : MonoBehaviour
{
    [SerializeField] TMP_InputField commandBoxInputField;
    [SerializeField] InputFieldActivator inputFieldActivator;

    void Update()
    {
        if (inputFieldActivator.IsActivated && Input.GetKeyDown(KeyCode.LeftControl))
        {
            commandBoxInputField.text = string.Empty;
        }

        if (inputFieldActivator.IsActivated && Input.GetKeyDown(KeyCode.Return))
        {
            CommandSet.ExecuteCommand(commandBoxInputField.text.ToLower());
            commandBoxInputField.text = string.Empty;
            commandBoxInputField.ActivateInputField();
        }
    }
}
