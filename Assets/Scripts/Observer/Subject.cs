using System.Collections.Generic;
using Data;
using UnityEngine;

namespace ObserverPattern
{
    public abstract class Subject : MonoBehaviour
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        protected void NotifyObserver(GameData gameData)
        {
            _observers.ForEach(observer => { observer.Notify(gameData); });
        }
    }
}


