/**
 * Implements the pursue steering behaviour.
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// The Pursue behaviour attempts to intercept what it is chasing by
/// looking at it's current velocity and seeking a target on that path.
/// </summary>
public class PursueSteer : SeekSteer
{
	public KinematicInfo LocalTarget;
	public float MaxPrediction = 3.0f;

	public PursueSteer()
	{
		LocalTarget = new KinematicInfo();
	}

	public PursueSteer(KinematicInfo kinematicInfo)
	{
		LocalTarget = kinematicInfo;
	}

	public PursueSteer(Agent agent)
	{
		LocalTarget = agent.KinematicInfo;
	}

	public override SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;
		Vector3 direction = LocalTarget.Position - info.Position;
		float distance = direction.magnitude;
		float speed = LocalTarget.Velocity.magnitude;

		float prediction = 0.0f;
		if (speed <= distance / MaxPrediction) {
			prediction = MaxPrediction;
		} else {
			prediction = distance / speed;
		}


		base.Target = LocalTarget;
		base.Target.Position += LocalTarget.Velocity * prediction;

		return base.CalculateAcceleration(agent);
	}
}