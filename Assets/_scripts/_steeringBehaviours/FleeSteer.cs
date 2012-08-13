/**
 * Implements the flee behaviour
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// The Flee behaviour tries to move a target as far away as possible
/// from it's target.
/// </summary>
public class FleeSteer : ISteeringBehaviour
{
	public KinematicInfo Target = new KinematicInfo();
	public float MaxAcceleration;

	public FleeSteer()
	{
	}

	virtual public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
	{
		SteeringOutput steering = new SteeringOutput();

		steering.Linear = info.Position - Target.Position;
		steering.Linear.Normalize();
		steering.Linear *= MaxAcceleration;

		return steering;
	}
}
