using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private TaquinController GlobTaquinController;
    private PreviewFrame GlobPreviewFrame;

    public float Timer;
    public Text TimerText;
    public GameObject WinText;
    public GameObject LoseText;
    [HideInInspector]
    public bool IsGameFinished;

    private bool IsGameStarted;

    private void Awake()
    {
        GlobTaquinController = FindObjectOfType<TaquinController>();
        GlobPreviewFrame = FindObjectOfType<PreviewFrame>();
    }

    private void Start()
    {
        GlobTaquinController.Initialize();
    }

    public void StartGame()
    {
        IsGameStarted = true;
    }

    private void Update()
    {
        if (!IsGameStarted)
        {
            return;
        }

        if (!IsGameFinished)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        Timer -= Time.deltaTime;

        int timerMin = (int)Timer / 60;
        int timerSec = (int)Timer % 60;

        TimerText.text = timerMin.ToString() + ":" + timerSec.ToString();

        if(Timer <= 0)
        {
            Lose();
        }
    }

    public void Win()
    {
        WinText.SetActive(true);
        GlobPreviewFrame.LastTile.SetActive(true);
        IsGameFinished = true;

        if (PlayerPrefs.GetFloat("HighScore") < Timer)
        {
            PlayerPrefs.SetFloat("HighScore", Timer);
        }
    }

    public void Lose()
    {
        LoseText.SetActive(true);
        IsGameFinished = true;
    }
}