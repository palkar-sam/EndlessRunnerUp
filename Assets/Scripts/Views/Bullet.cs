using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private List<BulletMaterials> materials;

    public CollectibleType CollectType => _type;

    private float _currentTime;
    private CollectibleType _type;

    public void InitData(float playerSpeed)
    {
        if (playerSpeed > 0)
            speed += playerSpeed;
    }

    public void ActivateBullet(CollectibleType type)
    {
        var bulletMat = materials.Find(bulletItem => bulletItem.Type == type);
        _type = type;

        if (bulletMat != null)
            meshRenderer.material = bulletMat.BulletMaterial;

        _currentTime = 0;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        _currentTime += Time.deltaTime;
        if(_currentTime >= resetTime)
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    { 
       gameObject.SetActive(false);
        
    }

    [Serializable]
    internal class BulletMaterials
    {
        [SerializeField] private CollectibleType type;
        [SerializeField] private Material material;

        public CollectibleType Type => type;
        public Material BulletMaterial => material;
    }
}

