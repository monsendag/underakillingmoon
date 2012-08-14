using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Seperation behaviour tries to keep agents heading in the same direction
/// from getting too close to each other.
/// </summary>
class SeperationSteer : ISteeringBehaviour
{
	public float LookAhead = 4.0f;
	public float DecayCoefficient = 1.0f;
	public float MaxAcceleration = 4.0f;
	public float Threshold = 0.75f;

	public SeperationSteer()
	{
	}

	virtual public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
	{
		SteeringOutput output = new SteeringOutput();
		output.Linear = Vector2.zero;
		output.Angular = 0.0f;
        
		var agentList = agent.GetComponent<Agent>().GetAgentsInArea(LookAhead);

		foreach (var a in agentList) {
			Vector2 direction = a.KinematicInfo.Position - info.Position;
			float distance = direction.magnitude;
			if (distance < Threshold) {
				float strength = Mathf.Min(DecayCoefficient / (distance * distance), 
                    MaxAcceleration);

				direction.Normalize();
				output.Linear -= strength * direction;
			}
		}

		return output;
	}
}
