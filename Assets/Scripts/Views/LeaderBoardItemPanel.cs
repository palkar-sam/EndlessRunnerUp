using TMPro;
using UnityEngine;

public class LeaderBoardItemPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetData(string name, string score)
    {
        nameText.text = name;
        scoreText.text = score;
        gameObject.SetActive(true);
    }
}
