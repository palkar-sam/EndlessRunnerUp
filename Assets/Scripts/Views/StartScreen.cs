using System;
using TMPro;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    public event Action OnStartGame;

    public void SetData(int highScore)
    {
        highScoreText.text = $"High Score : {highScore}";
        gameObject.SetActive(true);
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

}
