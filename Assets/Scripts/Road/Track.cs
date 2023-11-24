using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using ObserverPattern;
using UnityEngine;

public class Track : MonoBehaviour, IObserver
{
    [SerializeField] private int trackId;
    [SerializeField] private CheckPoint checkPoint;
    [SerializeField] private List<Collectible> collectibles;
    [SerializeField] private List<Obstacle> obstacles;
    [SerializeField] private GameObject collectibleCollectEffect;
    [SerializeField] private GameObject redObstacleBlastEffect;
    [SerializeField] private GameObject greenObstacleBlastEffect;
    [SerializeField] private GameObject blueObstacleBlastEffect;

    public int TrackId => trackId;

    public event Action<int, int> OnTrackComplete;
    public event Action<GameData> OnItemCollected; 

    public void ResetTrack(float newPos)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, newPos);
        collectibles.ForEach(collectible => collectible.ResetData());
        obstacles.ForEach(obstacle => obstacle.ResetData());
        StartCoroutine(ResetEffect(collectibleCollectEffect, 0f));
        StartCoroutine(ResetEffect(redObstacleBlastEffect, 0f));
        StartCoroutine(ResetEffect(greenObstacleBlastEffect, 0f));
        StartCoroutine(ResetEffect(blueObstacleBlastEffect, 0f));
    }

    public void Notify(GameData data)
    {
        if (data is CollectibleData collectedData)
        {
            collectibleCollectEffect.transform.position = collectedData.Position;
            collectibleCollectEffect.SetActive(true);
            StartCoroutine(ResetEffect(collectibleCollectEffect, 0.5f));

            OnItemCollected?.Invoke(data);
        }
        else if (data is ObstacleData obstacleData)
        {
            GameObject obj = redObstacleBlastEffect;

            if (obstacleData.Type == CollectibleType.Green)
                obj = greenObstacleBlastEffect;
            else if (obstacleData.Type == CollectibleType.Blue)
                obj = blueObstacleBlastEffect;

            obj.transform.position = obstacleData.Position;
            obj.SetActive(true);
            StartCoroutine(ResetEffect(obj, 0.5f));
        }
    }

    private IEnumerator ResetEffect(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        obj.transform.position = Vector3.zero;
        obj.SetActive(false);
    }

    private void Start()
    {
        checkPoint.OnCheckPointComplete += OnCheckPointComplete;
    }

    private void OnCheckPointComplete(int id)
    {
        OnTrackComplete?.Invoke(trackId, id);
    }

    private void OnEnable()
    {
        collectibles.ForEach(collectible => collectible.AddObserver(this));
        obstacles.ForEach(obstacle => obstacle.AddObserver(this));
    }

    private void OnDisable()
    {
        collectibles.ForEach(collectible => collectible.RemoveObserver(this));
        obstacles.ForEach(obstacle => obstacle.RemoveObserver(this));
    }

}
