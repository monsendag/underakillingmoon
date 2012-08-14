/**
 * Implements the seek behaviour
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// The SeekSteer behaviour creates the effect of a unit seeking a target.
/// </summary>
public class SeekSteer : ISteeringBehaviour
{
	public KinematicInfo Target = new KinematicInfo();
	public float MaxAcceleration;

	public SeekSteer()
	{
	}

	virtual public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
	{
		SteeringOutput steering = new SteeringOutput();
		steering.Linear = Target.Position - info.Position;
		steering.Linear.Normalize();
		steering.Linear *= MaxAcceleration;
		Debug.Log(steering.Linear);
		return steering;
	}
}
