using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    throw new System.Exception("More than 1 singleton found!");
                }

                if (instance == null)
                {
                    throw new System.Exception("No singleton found!");
                }
            }

            return instance;
        }
    }

    private static T instance;
}