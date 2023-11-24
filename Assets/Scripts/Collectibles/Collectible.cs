using Data;
using ObserverPattern;
using UnityEngine;
using UnityEngine.UIElements;

public class Collectible : Subject
{
    [SerializeField] private CollectibleType type;
    [SerializeField] private GameObject coinObject;
    [SerializeField] private bool IsRotate;
    [SerializeField] private float rotationSpeed = 2.0f;

    public void ResetData()
    {
        coinObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet")) return;

        NotifyObserver(new CollectibleData() { Type = this.type, Position = transform.position, IsAddCollectible = true });
        coinObject.SetActive(false);
    }

    private void Update()
    {
        if (IsRotate)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

}
