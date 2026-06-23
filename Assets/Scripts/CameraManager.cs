using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }
    public Camera playerCamera { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public void SetPlayerCamera(Camera camera)
    {
        playerCamera = camera;
    }

    public void SetCameraRotation(Quaternion rotation)
    {
        if (playerCamera == null)
            return;

        playerCamera.transform.rotation = rotation;
    }
}
