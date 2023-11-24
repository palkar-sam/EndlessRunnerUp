using Data;

namespace ObserverPattern
{
    public interface IObserver
    {
        public void Notify(GameData d);
    }
}


