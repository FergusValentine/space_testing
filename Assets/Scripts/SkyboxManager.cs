using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public static SkyboxManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private Transform skyboxDirection;

    public Quaternion GetSkyboxRotation()
    {
        return skyboxDirection.rotation;
    }

    public void SetSkyboxRotation(Quaternion rotation)
    {
        skyboxDirection.rotation = rotation;
    }
}
