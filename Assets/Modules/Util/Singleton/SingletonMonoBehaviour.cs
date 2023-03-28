namespace JSGCode.Util
{
    using UnityEngine;

    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Member
        private static T instance = null;
        private static object lockObject = new object();
        private static bool applicationQuit = false;
        #endregion

        #region Static
        public static T Instance
        {
            get
            {
                if (applicationQuit)
                {
                    return null;
                }

                // Double-checked locking
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = FindObjectOfType<T>();

                            if (instance == null)
                                instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region Method : Mono
        protected virtual void OnApplicationQuit()
        {
            applicationQuit = true;
        }
        #endregion
    }
}