using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject endPanel;
    [SerializeField] GameObject startPanel;

    public static SceneController Instance;
    public bool pause = true;
    public bool win { get; private set; }

    bool isStartPanelActive = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 0;
        startPanel.SetActive(true);
    }

    void Update()
    {
        if (isStartPanelActive)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            Pause(true);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startPanel.SetActive(false);
        isStartPanelActive = false;
        pause = false;
    }

    public void Pause(bool pause)
    {
        this.pause = pause;
        Time.timeScale = pause ? 0 : 1;

        if (pausePanel == null)
        {
            pausePanel = Data.FindInactiveObjectWithTag(Data.pausePanelTag);
        }

        pausePanel.SetActive(pause);
    }

    public void EndGame(bool win)
    {
        if (endPanel == null)
            endPanel = Data.FindInactiveObjectWithTag(Data.endPanelTag);

        if (endPanel == null)
            return;

        endPanel.SetActive(true);
        Time.timeScale = 0;
        pause = true;

        EndPanel endPanelComponent = endPanel.GetComponent<EndPanel>();

        if (endPanelComponent == null)
            return;

        if (!win)
        {
            endPanelComponent.SetTitle("YOU LOST!");
            endPanelComponent.SetTextColor(Color.red);
        }
        else
        {
            PlaceholderWinningSystem.Instance.ShowUI();
            GameManager.Instance.WinGame();
        }
    }

    public void RestartGame()
    {
        Scene miniGameScene = SceneManager.GetSceneByName(Data.gameSceneName);

        if (miniGameScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(miniGameScene).completed += (AsyncOperation unloadOp) =>
            {
                Debug.Log("Minigame scene unloaded");

                PlaceholderWinningSystem.Instance.LoadMiniGame(false);
            };
        }
        else
        {
            Debug.LogError("Minigame scene not found or not loaded");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
