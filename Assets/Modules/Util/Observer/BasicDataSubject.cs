namespace JSGCode.Util
{
    using System.Collections.Generic;

    public class BasicDataSubject<T> : IDataSubject<T> where T : new()
    {
        private List<IDataObserver<T>> observers;

        public BasicDataSubject()
        {
            observers = new List<IDataObserver<T>>(); ;
        }

        public void AddObserver(IDataObserver<T> observer)
        {
            if (observers.Contains(observer) == false)
                observers.Add(observer);
        }

        public void NotifyObservers(T data)
        {
            foreach (var controller in observers.ToArray())
                controller.Notify(data);
        }

        public void RemoveObserver(IDataObserver<T> observer)
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
        }
    }
}