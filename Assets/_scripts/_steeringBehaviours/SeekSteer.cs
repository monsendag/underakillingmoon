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
	public float MaxAcceleration = 4.0f;

	public SeekSteer()
	{
		Target = new KinematicInfo();
	}

	public SeekSteer(KinematicInfo kinematicInfo)
	{
		Target = kinematicInfo;
	}

	public SeekSteer(Agent agent)
	{
		Target = agent.KinematicInfo;
	}

	virtual public SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;
		SteeringOutput steering = new SteeringOutput();
		steering.Linear = Target.Position - info.Position;
		steering.Linear.Normalize();
		steering.Linear *= MaxAcceleration;
		Debug.Log(steering.Linear);
		return steering;
	}
}
