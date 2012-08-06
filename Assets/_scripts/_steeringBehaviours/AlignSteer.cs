/**
 * Implements basic wander algorithm.
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// Steers towards a targets orientation
/// The AlignSteer behaviour creates the effect of a wandering unit.
/// </summary>
public class AlignSteer : ISteeringBehaviour
{
    public KinematicInfo Target = new KinematicInfo();
    public float MaxAngularAcceleration = 2.0f;
    public float MaxRotation = 7.0f;
    public float TargetRadius = 0.025f;
    public float SlowRadius = 0.05f;
    public float TimeToTarget = 0.1f;

    public AlignSteer() {}

    virtual public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
    {

        float rotation = Target.Orientation - info.Orientation;

        rotation = MotionUtils.MapToRangeRadians(rotation);
        

        float rotationSize = Mathf.Abs(rotation);

        SteeringOutput steering = new SteeringOutput();

        if (rotationSize < TargetRadius)
        {
            return steering;
        }

        float targetRotation = 0.0f;

        if (rotationSize > SlowRadius) { targetRotation = MaxRotation; }
        else
        {
            targetRotation = MaxRotation * rotationSize / SlowRadius;
        }

        targetRotation *= rotation / rotationSize;

        steering.Angular = targetRotation - info.AngularVelocity;
        steering.Angular /= TimeToTarget;

        float angularAcceleration = Mathf.Abs(steering.Angular);
        if (angularAcceleration > MaxAngularAcceleration)
        {
            steering.Angular /= angularAcceleration;
            steering.Angular *= MaxAngularAcceleration;
        }

        Debug.Log(angularAcceleration + " " + rotation + " " + targetRotation + " " + steering.Angular);

        return steering;
    }
}
