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
	public KinematicInfo Target;
	public float MaxAcceleration = 4.0f;
	public float TargetRadius = 0.1f;
	public float MaxVelocity = 4.0f;
	public float SlowRadius = 4.5f;
	public float TimeToTarget = 0.1f;

	public ArriveSteer()
	{
		Target = new KinematicInfo();
	}

	public ArriveSteer(KinematicInfo kinematicInfo)
	{
		Target = kinematicInfo;
	}

	public ArriveSteer(Agent agent)
	{
		Target = agent.KinematicInfo;
	}

	virtual public SteeringOutput CalculateAcceleration(Agent agent)
	{
		SteeringOutput steering = new SteeringOutput();

		KinematicInfo info = agent.KinematicInfo;

		Vector2 direction = Target.Position - info.Position;
		float distance = direction.magnitude;
		if (distance < TargetRadius) {
			return steering;
		}
        
		float targetSpeed = 0.0f;
		if (distance > SlowRadius) {
			targetSpeed = MaxVelocity;
		} else {
			targetSpeed = MaxVelocity * distance / SlowRadius;
		}

		Vector2 targetVelocity = direction;
		targetVelocity.Normalize();
		targetVelocity *= targetSpeed;
		steering.Linear = targetVelocity - info.Velocity;
		steering.Linear /= TimeToTarget;

		if (steering.Linear.magnitude > MaxAcceleration) {
			steering.Linear.Normalize();
			steering.Linear *= MaxAcceleration;
		}

		return steering;
	}
}
