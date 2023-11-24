using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private int checkPointId;

    public event Action<int> OnCheckPointComplete;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            OnCheckPointComplete?.Invoke(checkPointId);
    }
}
