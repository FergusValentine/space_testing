using UnityEngine;

namespace SpaceCoop.ShipSystems.ShipControl
{
    public class ClientView
    {
        public void OnEnable(Transform origin)
        {
            Cursor.lockState = CursorLockMode.None;
            CameraManager.instance.playerCamera.transform.rotation = Quaternion.LookRotation(origin.forward, origin.up);
        }

        public void OnDisable()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}