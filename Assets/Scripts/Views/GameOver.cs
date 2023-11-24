using System;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI newScoreText;
    [SerializeField] private TextMeshProUGUI tryAgainText;

    public event Action OnRestartGame;

    public void SetData(int score, int highScore, bool isNewScore)
    {
        scoreText.text = $"Your Score : {score}";
        highScoreText.text = $"High Score  : {highScore}";

        if (isNewScore)
        {
            newScoreText.gameObject.SetActive(true);
            tryAgainText.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        gameObject.SetActive(false);
        OnRestartGame?.Invoke();
    }

}
