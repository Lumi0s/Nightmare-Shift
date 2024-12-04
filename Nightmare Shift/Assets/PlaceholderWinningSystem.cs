using UnityEngine;
using UnityEngine.SceneManagement;
using static Data;

public class PlaceholderWinningSystem : MonoBehaviour
{
    public static PlaceholderWinningSystem Instance { get; private set; }

    [SerializeField] private KeyCode openUI;
    // cannot serialize objects from different scenes, we're gonna find it in the hierarchy on Start()
    [SerializeField] public GameObject minigameUI;

    public bool minigameActive = false;

    Camera mainCamera;
    Camera miniGameCamera;

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
        LoadMiniGame(true);
    }

    public void LoadMiniGame(bool firstTime)
    {
        GameObject mainCameraGO = GameObject.FindWithTag("MainCamera");
        mainCamera = mainCameraGO.GetComponent<Camera>();

        SceneManager.LoadSceneAsync(Data.gameSceneName, LoadSceneMode.Additive).completed += (AsyncOperation op) =>
        {
            Initialize(firstTime);
        };
    }

    void Initialize(bool firstTime)
    {
        GameObject miniGame = Data.FindInactiveObjectWithTag(Data.miniGameMainObjectTag);
        if (miniGame != null)
        {
            minigameUI = miniGame;
            miniGame.SetActive(firstTime ? false : true);
            GameObject cameraGO = Data.FindInactiveObjectWithTag(Data.miniGameCameraTag);

            if (cameraGO != null)
                miniGameCamera = cameraGO.GetComponent<Camera>();
            else
                Debug.LogError("No minigame camera object found");
        }
        else
        {
            Debug.LogError("No minigame object found");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(openUI))
        {
            ShowUI();
        }
    }

    public void ShowUI()
    {
        minigameActive = !minigameActive;
        minigameUI.SetActive(minigameActive);
        mainCamera.enabled = !minigameActive;
        miniGameCamera.enabled = minigameActive;
        SoundManager.Instance.PlaySound("CamerasDown");
    }
}
