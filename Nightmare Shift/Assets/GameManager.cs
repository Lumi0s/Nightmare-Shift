using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [HideInInspector] public bool lostGame = false;
    [SerializeField] private GameObject GameOverCamera;
    private GameObject mainCamera;

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

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        mainCamera.SetActive(false);
        GameOverCamera.SetActive(true);
        PowerSystem.Instance.usageUI.gameObject.SetActive(false);
        PowerSystem.Instance.powerUI.gameObject.SetActive(false);
        SoundManager.Instance.PlaySound("GameOver");
    }

    public void WinGame()
    {
        mainCamera.SetActive(false);
        GameOverCamera.SetActive(true);
        PowerSystem.Instance.usageUI.gameObject.SetActive(false);
        PowerSystem.Instance.powerUI.gameObject.SetActive(false);
        SoundManager.Instance.PlaySound("Win");
    }
}