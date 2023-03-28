namespace JSGCode.Util
{
    using System;

    public class Singleton<T> where T : class, new()
    {
        #region Static
        private static readonly Lazy<T> instance = new Lazy<T>(() => new T(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        public static T Instance => instance.Value;

        public static bool TryGetInstance(out T instance)
        {
            instance = Instance;
            return instance != null;
        }
        #endregion

        #region Constructor
        protected Singleton() { }
        #endregion
    }
}