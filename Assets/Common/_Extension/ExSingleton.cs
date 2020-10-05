namespace Common.Singleton
{
    using UnityEngine;
    public enum EtypeSingleton { DontDestroyOnLoad, DestroyOnLoad }
    public class ExSingleton<T> : MonoBehaviour where T : Component
    {
        static T _instance;
        public static T instance { get { return _instance; } }
        public EtypeSingleton typeSingleton;
        public virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this as T;

            switch (typeSingleton)
            {
                case EtypeSingleton.DestroyOnLoad: break;
                default:
                    DontDestroyOnLoad(this);
                    break;
            }

        }
    }
}
