using System.Collections.Generic;
using Data;
using ObserverPattern;
using UnityEngine;

public class Road : Subject, IObserver
{
    [SerializeField] private Subject uiSubject;
    [SerializeField] private List<Track> tracks;
    [SerializeField] private float trackPositionVal = 30.0f;
    [SerializeField] private float lastTrackValue = 82;

    private Queue<Track> _tracksQueue = new Queue<Track>();
    private List<Vector3> _trackInitialPos = new List<Vector3>();
    private float _defaultLastTrackValue;


    private void OnEnable()
    {
        uiSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        uiSubject.RemoveObserver(this);
    }

    private void ResetTracks()
    {
        lastTrackValue = _defaultLastTrackValue;
        _tracksQueue.Clear();
        for (int i = 0; i < tracks.Count; i++)
        {
            tracks[i].transform.position = _trackInitialPos[i];
        }
    }

    private void Start()
    {
        _defaultLastTrackValue = lastTrackValue;
        for (int i = 0; i < tracks.Count; i++)
        {
            tracks[i].OnTrackComplete += OnTrackComplete;
            tracks[i].OnItemCollected += OnItemCollected;
            _trackInitialPos.Add(tracks[i].transform.position);
        }
    }

    private void OnTrackComplete(int trackId, int checkPointId)
    {
        //Debug.Log("TrackCompleted - track Id : " + trackId + " : Total Queue : " + tracksQueue.Count+" : called Count : ");
        

        Track track = tracks.Find(trackItem => trackItem.TrackId == trackId);
        _tracksQueue.Enqueue(track);

        if (_tracksQueue.Count > 1)
        {
            track = _tracksQueue.Dequeue();
            lastTrackValue = lastTrackValue + trackPositionVal;
            track.ResetTrack(lastTrackValue);
        }
    }

    private void OnItemCollected(GameData d)
    {
        NotifyObserver(d);
    }

    public void Notify(GameData data)
    {

        Debug.Log("------------- Recieve Notification in Road ------------");
        if (data.IsStartNewGame || !data.IsGameOver)
        {
            Debug.Log("------------- Restting tracks value as New Game is started------------");
            ResetTracks();
        }
    }
}
