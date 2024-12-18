using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] Button playButton;

    void Start()
    {
        playButton.onClick.AddListener(() => StartGame());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        SceneController.Instance.StartGame();
    }
}
