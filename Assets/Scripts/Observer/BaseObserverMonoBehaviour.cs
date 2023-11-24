using Data;
using UnityEngine;

namespace ObserverPattern
{
    public class BaseObserverMonoBehaviour : MonoBehaviour, IObserver
    {
        public void Notify(GameData data)
        {
            throw new System.NotImplementedException();
        }
    }
}


