using System.Collections.Generic;
using Data;
using ObserverPattern;
using UnityEngine;

public class PlayerAttak : Subject
{    
    [SerializeField] private Transform firePoint;
    [SerializeField] private List<Bullet> bullets;

    public float PlayerSpeed { get; set; }

    private bool _fireBullet;

    private void Start()
    {
        bullets.ForEach(bullet => bullet.InitData(PlayerSpeed));
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) || _fireBullet)
        {
            _fireBullet = false;

            if (RunnerInventoryData.GetInstance().IsBulletAvailable)
            {
                CollectibleType type = RunnerInventoryData.GetInstance().GetRanomBullet();

                if(type != CollectibleType.None)
                {
                    int index = bullets.FindIndex(bullet => !bullet.gameObject.activeInHierarchy);

                    if (index > -1)
                    {
                        bullets[index].transform.position = firePoint.position;
                        bullets[index].ActivateBullet(type);
                    }

                    NotifyObserver(new CollectibleData() { Type = type, Position = transform.position, IsReduceCollectible = true });
                }
                
            }
        }
    }

    public void FireBullet()
    {
        _fireBullet = true;
    }
}
