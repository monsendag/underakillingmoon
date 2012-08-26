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
	public KinematicInfo Target;
	public float MaxRotation = 7.0f;
	public float TargetRadius = 0.025f;
	public float SlowRadius = 0.05f;
	public float TimeToTarget = 0.1f;

	public AlignSteer()
	{
		Target = new KinematicInfo();
	}

	public AlignSteer(KinematicInfo kinematicInfo)
	{
		Target = kinematicInfo;
	}

	public AlignSteer(Agent agent)
	{
		Target = agent.KinematicInfo;
	}

	virtual public SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;

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
