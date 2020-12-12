using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private Text HighScoreText;

    private void Awake()
    {
        HighScoreText = GetComponent<Text>();

        if (PlayerPrefs.GetFloat("HighScore") != default)
        {
            float highScore = 180 - PlayerPrefs.GetFloat("HighScore");

            int timerMin = (int)highScore / 60;
            int timerSec = (int)highScore % 60;

            HighScoreText.text = timerMin.ToString() + ":" + timerSec.ToString();
        }
    }
}