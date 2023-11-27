using UnityEngine;
using TMPro;

public class RunnerLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    private static RunnerLog _instance;
    private static string _logStr = string.Empty;

    private bool _isShown;
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        
    }

    public static void Log(string str)
	{
		Debug.Log(str);
        _logStr = string.Concat(_logStr, "\n", str);

        if(_instance != null)
            _instance.ShowLogs();
    }

    public void ShowLogPanel()
    {
        _isShown = !_isShown;
        highScoreText.gameObject.SetActive(_isShown);
        ShowLogs();
    }

    private void ShowLogs()
    {
        if(highScoreText.gameObject.activeInHierarchy)
            highScoreText.text = _logStr;
    }    
}

