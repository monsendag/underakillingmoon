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

	public EvadeSteer()
	{
		LocalTarget = new KinematicInfo();
	}

	public EvadeSteer(KinematicInfo kinematicInfo)
	{
		LocalTarget = kinematicInfo;
	}

	public EvadeSteer(Agent agent)
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


		base.Target.Position = LocalTarget.Position;
		base.Target.Position += LocalTarget.Velocity * prediction;

		return base.CalculateAcceleration(agent);
	}
}