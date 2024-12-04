using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject endPanel;
    [SerializeField] GameObject startPanel;

    [SerializeField] Canvas canvas;


    public static SceneController Instance;
    public bool pause { get; private set; } = true;
    public bool win { get; private set; } = default;

    bool isStartPanelActive = true;

    private MoveCamera mainCameraComponent;

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
        pause = true;
        //startPanel.SetActive(true);
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        mainCameraComponent = mainCamera.GetComponent<MoveCamera>();
        canvas.enabled = true;
        pausePanel.SetActive(true);

    }

    void Update()
    {
        if (mainCameraComponent.isMoving)
        {
            return;
        }
        if (isStartPanelActive)
            return;

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(PlaceholderWinningSystem.Instance.openUI)) && win == default)
        {
            Pause(true);
        }
    }

    public void StartGame()
    {
        if (mainCameraComponent.isMoving)
        {
            return;
        }
        startPanel.SetActive(false);
        isStartPanelActive = false;
        pause = false;
    }

    public void Pause(bool pause)
    {
        this.pause = pause;

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

    public void CanvasVisibility(bool visible)
    {
        canvas.enabled = visible;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
