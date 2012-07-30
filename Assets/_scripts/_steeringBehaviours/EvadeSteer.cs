/**
 * Implements the evade steering behaviour.
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// The evade behaviour moves away from where the target will be, based on it's
/// velocity.
/// </summary>
public class EvadeSteer : FleeSteer
{
    public KinematicInfo LocalTarget = new KinematicInfo();
    public float MaxPrediction = 3.0f;

    public EvadeSteer() { }

    public override SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
    {
        Vector3 direction = LocalTarget.Position - info.Position;
        float distance = direction.magnitude;
        float speed = LocalTarget.Velocity.magnitude;

        float prediction = 0.0f;
        if (speed <= distance / MaxPrediction)
        {
            prediction = MaxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }


        base.Target = LocalTarget;
        base.Target.Position += LocalTarget.Velocity * prediction;

        return base.CalculateAcceleration(agent,info);
    }
}