using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] Button returnButton;

   void Start()
    {
        returnButton.onClick.AddListener(() => ReturnToGame());
    }

    void ReturnToGame()
    {
        SceneController.Instance.Pause(false);
    }
}