using UnityEngine;

public class BaseSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject($"Singleton of {typeof(T).Name}");
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this as T)
            instance = null;
    }
}
