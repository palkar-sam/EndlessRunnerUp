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
    [SerializeField] private GameObject ObstacleBlastEffect;

    public int TrackId => trackId;

    public event Action<int, int> OnTrackComplete;
    public event Action<GameData> OnItemCollected; 

    public void ResetTrack(float newPos)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, newPos);
        collectibles.ForEach(collectible => collectible.ResetData());
        obstacles.ForEach(obstacle => obstacle.ResetData());
        StartCoroutine(ResetEffect(collectibleCollectEffect, 0f));
        StartCoroutine(ResetEffect(ObstacleBlastEffect, 0f));
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
            ObstacleBlastEffect.transform.position = obstacleData.Position;
            ObstacleBlastEffect.SetActive(true);
            StartCoroutine(ResetEffect(ObstacleBlastEffect, 0.5f));
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
