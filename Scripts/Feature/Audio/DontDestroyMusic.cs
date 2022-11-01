using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    public static DontDestroyMusic instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
            return;
        }
    }
}
