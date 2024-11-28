using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Profiling;
using UnityEngine;

public class PlaceholderWinningSystem : MonoBehaviour
{

    public static PlaceholderWinningSystem Instance { get; private set; }
    [SerializeField] private KeyCode openUI;
    [SerializeField] public GameObject minigameUI;
    public TextMeshProUGUI progressText;
    private float progress = 0.0f;
    public bool minigameActive = false;

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
        
    }
    // Update is called once per frame
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
            SoundManager.Instance.PlaySound("CamerasDown");
    }

    public void IncreaseProgress()
    {
        progress += Time.deltaTime*0.65f;
        progressText.text = "Progress: " + progress.ToString("N0") + "%";
        if (progress >= 100)
        {
            ShowUI();
            GameManager.Instance.WinGame();
            GameManager.Instance.lostGame = true;
        }
    }
}
