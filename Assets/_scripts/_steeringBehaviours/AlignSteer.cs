/**
 * Implements basic align algorithm.
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
	public float MaxRotation = Config.DefaultMaxRotation;
	public float TargetRadius = Config.DefaultAlignTargetRadius;
	public float SlowRadius = Config.DefaultAlignSlowRadius;
	public float TimeToTarget = Config.DefaultAlignTimeToTarget;

	public AlignSteer()
	{
	}

	virtual public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
	{
		float rotation = Target.Orientation - info.Orientation;
        
		rotation = MotionUtils.MapToRangeRadians(rotation);
        

		float rotationSize = Mathf.Abs(rotation);

		SteeringOutput steering = new SteeringOutput();
		steering.Linear = Vector3.zero;

		if (rotationSize < TargetRadius) {
			return steering;
		}

		float targetRotation = 0.0f;

		if (rotationSize > SlowRadius) {
			targetRotation = MaxRotation;
		} else {
			targetRotation = MaxRotation * rotationSize / SlowRadius;
		}

		targetRotation *= rotation / rotationSize;

		steering.Angular = targetRotation - info.AngularVelocity;
		steering.Angular /= TimeToTarget;

		return steering;
	}
}
