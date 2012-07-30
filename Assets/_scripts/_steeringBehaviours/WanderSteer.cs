/**
 * Implements basic wander algorithm.
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// The WanderSteer behaviour creates the effect of a wandering unit.
/// </summary>
public class WanderSteer : FaceSteer
{
    public float MaxAcceleration = 0.0f;
    public float WanderOffset = 0.0f;
    public float WanderRadius = 0.0f;
    public float MaxOrientationChange = 0.0f;
    public float WanderOrientation = 0.0f;

    public WanderSteer() {}

    override public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
    {
        //  Weight the change in orientation  against the angular velocity of info.
        WanderOrientation += MaxOrientationChange * 
            MotionUtils.RandomBinomial() - info.AngularVelocity * Random.Range(0.0f,1.0f);

        float targetOrientation = WanderOrientation + info.Orientation;

        Vector2 target = info.Position + WanderOffset * 
            MotionUtils.GetOrientationAsVector(info.Orientation);
        target += WanderRadius * 
            MotionUtils.GetOrientationAsVector(targetOrientation);

        Vector3 target3d = new Vector3(target.x - info.Position.x, 0.0f,target.y - info.Position.y);
        Debug.DrawLine(agent.transform.position, agent.transform.position + target3d, Color.green);

        // Get the steering output from the face behaviour

        base.LocalTarget.Position = target;

        SteeringOutput steering = base.CalculateAcceleration(agent, info);

        steering.Linear = MaxAcceleration * 
            MotionUtils.GetOrientationAsVector(info.Orientation);

        return steering;
    }
}
