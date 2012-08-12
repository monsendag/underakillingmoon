/**
 * Implements the arrive behaviour
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// The Arrive behaviour moves towards a target, and slows down as it arrives.
/// </summary>
public class ArriveSteer : ISteeringBehaviour
{
    public KinematicInfo Target = new KinematicInfo();
    public float MaxAcceleration;
    public float TargetRadius;
    public float MaxVelocity;
    public float SlowRadius = 0.5f;
    public float TimeToTarget = 0.1f;

    public ArriveSteer() {}

    virtual public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
    {
        SteeringOutput steering = new SteeringOutput();

        Vector2 direction = Target.Position - info.Position;
        float distance = direction.magnitude;
        if (distance < TargetRadius)
        {
            return steering;
        }
        
        float targetSpeed = 0.0f;
        if (distance > SlowRadius)
        {
            targetSpeed = MaxVelocity;
        }
        else
        {
            targetSpeed = MaxVelocity * distance / SlowRadius;
        }

        Vector2 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;
        steering.Linear = targetVelocity - info.Velocity;
        steering.Linear /= TimeToTarget;

        if (steering.Linear.magnitude > MaxAcceleration)
        {
            steering.Linear.Normalize();
            steering.Linear *= MaxAcceleration;
        }

        return steering;
    }
}
