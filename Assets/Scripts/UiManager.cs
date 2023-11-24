using Data;
using ObserverPattern;
using UnityEngine;

public class UiManager : Subject, IObserver
{
    [SerializeField] private Subject roadSubject;
    [SerializeField] private Subject playerSubject;
    [SerializeField] private Subject playerAttackSubject;
    [SerializeField] private BulletInventoryView bulletInventory;
    [SerializeField] private ScoreView playerScore;
    [SerializeField] private HealthBarView playerLife;
    [SerializeField] private GameOver gameOver;
    [SerializeField] private StartScreen startScreen;
    [SerializeField] private RunnerLog runnerLog;
    [SerializeField] private LeaderBoardScreen leaderBoardScreen;
    [SerializeField] private GameObject leaderBoardButton;

    private int _totalDistanceCovered;

    private void Awake()
    {
        startScreen.OnStartGame += OnStartGame;
        gameOver.OnRestartGame += OnRestartGame;
        //runnerLog.OnShowLog += OnShowLog;
        RunnerInventoryData.GetInstance().SetLeaderBoardData();
    }

    private void Start()
    {
        OnRestartGame();

        
    }

    private void OnEnable()
    {
        roadSubject.AddObserver(this);
        playerSubject.AddObserver(this);
        playerAttackSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        roadSubject.RemoveObserver(this);
        playerSubject.RemoveObserver(this);
        playerAttackSubject.RemoveObserver(this);
        RunnerInventoryData.GetInstance().SaveLeaderBoardData();
    }

    private void OnShowLog()
    {
       //runnerLog.SetLog(RunnerInventoryData.GetInstance().DebugData);
    }

    private void OnRestartGame()
    {
        startScreen.SetData(RunnerInventoryData.GetInstance().HighScore);
        playerLife.ResetLife();
        _totalDistanceCovered = 0;
        if (RunnerInventoryData.GetInstance().LeaderBoardList.Count > 0)
        {
            leaderBoardButton.SetActive(true);
        }
    }
    private void OnStartGame()
    {
        RunnerInventoryData.GetInstance().CreateCurrentRoundId();
        startScreen.gameObject.SetActive(false);
        NotifyObserver(new GameData { IsStartNewGame = true });
    }

    public void ShowLeaderBoard()
    {
        leaderBoardScreen.SetData(RunnerInventoryData.GetInstance().LeaderBoardList);
    }

    public void Notify(GameData d)
    {
        if (d.DistanceCovered > 0)
        {
            if (d.DistanceCovered > _totalDistanceCovered)
                _totalDistanceCovered = d.DistanceCovered;

            playerScore.AddDistance(d.DistanceCovered);
        }
        else if(d.IsDamagePlayer)
        {
            playerLife.DamageLife(out int playerLifeCount);
            Debug.Log("Ui Manager : life Count : " + playerLifeCount);
            if (playerLifeCount < 0)
            {
                NotifyObserver(new GameData() { IsGameOver = true });

                RunnerInventoryData.GetInstance().SetHighestScore(_totalDistanceCovered);
                gameOver.SetData(_totalDistanceCovered, RunnerInventoryData.GetInstance().HighScore, false);
            }
            else
            {
                NotifyObserver(new GameData() { IsGameOver = false });
            }
        }
        else
        {
            if (d is CollectibleData data)
            {

                //Debug.Log("Data : Add : " + data.IsAddCollectible + " Reduce : " + data.IsReduceCollectible);

                if (data.IsAddCollectible)
                    bulletInventory.AddBullet(data.Type, 1, true);
                else if (data.IsReduceCollectible)
                    bulletInventory.AddBullet(data.Type, 1, false);

                switch (data.Type)
                {
                    case CollectibleType.Red:
                        if (data.IsAddCollectible)
                            RunnerInventoryData.GetInstance().AddRedBullet();
                        else if (data.IsReduceCollectible)
                            RunnerInventoryData.GetInstance().reduceRedBullet();
                        break;

                    case CollectibleType.Green:
                        if (data.IsAddCollectible)
                            RunnerInventoryData.GetInstance().AddGreenBullet();
                        else if (data.IsReduceCollectible)
                            RunnerInventoryData.GetInstance().reduceGreenBullet();
                        break;

                    case CollectibleType.Blue:
                        if (data.IsAddCollectible)
                            RunnerInventoryData.GetInstance().AddBlueBullet();
                        else if (data.IsReduceCollectible)
                            RunnerInventoryData.GetInstance().reduceBlueBullet();
                        break;
                }
            }
        }
    }
}
