using UnityEngine;

[CreateAssetMenu(fileName = "ShipScriptableObject", menuName = "Scriptable Objects/ShipScriptableObject")]
public class ShipScriptableObject : ScriptableObject
{
    public Vector3 maxLinearVelocity;
    public Vector3 linearAcceleration;
    public Vector3 linearDeceleration;

    public Vector3 maxRotationalVelocity;
    public Vector3 rotationalAcceleration;
    public Vector3 rotationalDecelleration;
}
