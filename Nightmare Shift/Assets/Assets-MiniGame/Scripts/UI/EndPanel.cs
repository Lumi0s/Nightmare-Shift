using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField] Button restartGameButton;
    [SerializeField] TextMeshProUGUI title;

    void Start()
    {
        restartGameButton.onClick.AddListener(() => RestartGame());
    }

    public void SetTitle(string text)
    {
        title.text = text;
    }

    public void SetTextColor(Color color)
    {
        title.color = color;
    }

    void RestartGame()
    {
        SceneController.Instance.RestartGame();
    }
}
