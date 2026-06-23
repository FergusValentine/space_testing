using Unity.Netcode;
using UnityEngine;

namespace SpaceCoop.ShipSystems.ShipControl
{
    public class ShipView : NetworkBehaviour
    {
        [SerializeField] private Transform shipWheel;

        //public void Update(ShipPhysicsModel model, Transform transform, Transform environment)
        //{
        //    foreach (Transform child in environment)
        //    {
        //        Matrix4x4 environmentSpace = transform.worldToLocalMatrix * child.localToWorldMatrix;
        //        Matrix4x4 newEnvironmentSpace = model.shipSpace * environmentSpace;

        //        child.position = newEnvironmentSpace.GetPosition();
        //        child.rotation = Quaternion.Normalize(newEnvironmentSpace.rotation);
        //    }
        //}

        public void UpdateSteering(Vector3 movementInput, Vector3 steeringInput)
        {
            float step = 50f * Time.deltaTime;

            Vector3 localTarget = transform.InverseTransformDirection(movementInput);
            float targetY = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            // Rotate around the wheel's own local Y axis, preserving the 315 tilt
            Quaternion baseTilt = Quaternion.Euler(315f, 0f, 0f);
            Quaternion yRotation = Quaternion.AngleAxis(targetY, Vector3.up);
            Quaternion goalRotation = baseTilt * yRotation;

            shipWheel.localRotation = Quaternion.RotateTowards(shipWheel.localRotation, goalRotation, step);
        }
    }
}