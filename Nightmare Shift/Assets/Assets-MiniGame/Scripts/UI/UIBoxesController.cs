using UnityEngine;

public class UIBoxesController : MonoBehaviour
{
    [SerializeField] UserInterfaceItemsActivator leftBox;
    [SerializeField] UserInterfaceItemsActivator rightBox;
    [SerializeField] UserInterfaceItemsActivator topBox;

    void Start()
    {
        Init();
    }

    void Init()
    {
        leftBox.SetActivationState(false);
        rightBox.SetActivationState(false);
        topBox.SetActivationState(true);
    }

    void Update()
    {
        if (SceneController.Instance.pause)
        {
            Init();
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftBox.SetActivationState(true);
            rightBox.SetActivationState(false);
            topBox.SetActivationState(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            leftBox.SetActivationState(false);
            rightBox.SetActivationState(true);
            topBox.SetActivationState(false);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            leftBox.SetActivationState(false);
            rightBox.SetActivationState(false);
            topBox.SetActivationState(true);
        }

    }
}
