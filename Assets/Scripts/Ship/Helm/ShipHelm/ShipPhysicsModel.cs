using System;
using UnityEngine;

public class ShipPhysicsModel
{
    public Vector3 movementInput;
    public Vector2 steeringInput;
    public float rollInput;

    public Vector3 maxLinearVelocity;
    public Vector3 linearAcceleration;
    public Vector3 linearDeceleration;
    public Vector3 linearVelocity;

    public Vector3 maxRotationalVelocity;
    public Vector3 rotationalAcceleration;
    public Vector3 rotationalDecelleration;
    public Vector3 rotationalVelocity;
      
    public Matrix4x4 shipSpace;

    public ShipPhysicsModel(ShipScriptableObject data)
    {
        maxLinearVelocity = data.maxLinearVelocity;
        linearAcceleration = data.linearAcceleration;
        linearDeceleration = data.linearDeceleration;

        maxRotationalVelocity = data.maxRotationalVelocity;
        rotationalAcceleration = data.rotationalAcceleration;
        rotationalDecelleration = data.rotationalDecelleration;
    }

    private float AccelerateVector(float movement, float velocity, float acceleration, float deceleration, float max)
    {
        if (Mathf.Abs(movement) > 0.05f)
            velocity += movement * acceleration * Time.deltaTime;
        else
            velocity = Mathf.MoveTowards(velocity, 0.0f, deceleration * Time.deltaTime);
        return Mathf.Clamp(velocity, -max, max);
    }

    public void Process(Transform transform)
    {
        Vector3 controlInput = new Vector3(-movementInput.x, -movementInput.y, -movementInput.z).normalized;
        Vector3 movementDirection = transform.TransformDirection(controlInput);

        linearVelocity.x = AccelerateVector(movementDirection.x, linearVelocity.x, linearAcceleration.x, linearDeceleration.x, maxLinearVelocity.x);
        linearVelocity.y = AccelerateVector(movementDirection.y, linearVelocity.y, linearAcceleration.y, linearDeceleration.y, maxLinearVelocity.y);
        linearVelocity.z = AccelerateVector(movementDirection.z, linearVelocity.z, linearAcceleration.z, linearDeceleration.z, maxLinearVelocity.z);

        Vector3 steering = new Vector3(steeringInput.y, -steeringInput.x, rollInput);

        rotationalVelocity.x = AccelerateVector(steering.x, rotationalVelocity.x, rotationalAcceleration.x, rotationalDecelleration.x, maxRotationalVelocity.x);
        rotationalVelocity.y = AccelerateVector(steering.y, rotationalVelocity.y, rotationalAcceleration.y, rotationalDecelleration.y, maxRotationalVelocity.y);
        rotationalVelocity.z = AccelerateVector(steering.z, rotationalVelocity.z, rotationalAcceleration.z, rotationalDecelleration.z, maxRotationalVelocity.z);

        Matrix4x4 offset = Matrix4x4.TRS(linearVelocity * Time.deltaTime, Quaternion.Euler(rotationalVelocity * Time.deltaTime), Vector3.one);
        shipSpace = offset * transform.localToWorldMatrix;
        
        ///Aledgedly we need to change this to accumulation unless we want error
    }
}
