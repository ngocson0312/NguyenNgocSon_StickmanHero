using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError("Instance doesn't exist");
                    return null;
                }
                return instance;
            }
            else
            {
                return instance;
            }
        }
    }
    protected virtual void Awake()
    {
        instance = GetComponent<T>();
    }
    protected virtual void OnDestroy()
    {
        instance = null;
    }
}
