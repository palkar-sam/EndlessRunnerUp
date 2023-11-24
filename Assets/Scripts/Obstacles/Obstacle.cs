using Data;
using ObserverPattern;
using UnityEngine;

public class Obstacle : Subject
{
    [SerializeField] private CollectibleType obstacleType;

    public void ResetData()
    {
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            Bullet b = collision.gameObject.GetComponent<Bullet>();
            if (b != null && b.CollectType == obstacleType)
            {
                gameObject.SetActive(false);
                NotifyObserver(new ObstacleData() { Position = transform.position });
            }
        }
    }
}
