/**
 * Implements basic frictionalgorithm
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// Applies a force to slow down the current velocity or angular velocity..
/// </summary>
public class FrictionSteer : ISteeringBehaviour
{
	// This variable represents the rate of inertia
	public float VelocityFrictionPercentage = 0.1f;
	public float AngularVelocityFrictionPercentage = 0.1f;

	public FrictionSteer()
	{
	}

	public SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;

		SteeringOutput output = new SteeringOutput();
		output.Linear = info.Velocity * (-VelocityFrictionPercentage);
		output.Angular = info.AngularVelocity * (-AngularVelocityFrictionPercentage);
        if (agent.GetComponent<PlayerMovement>() != null)
        {

            Debug.Log("YELLOW " + info.Velocity + " " + VelocityFrictionPercentage + " "  + output.Linear);
        }
		return output;
	}
}