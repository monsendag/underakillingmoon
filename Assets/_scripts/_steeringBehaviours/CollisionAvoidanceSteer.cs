/**
 * Collision Avoidance steer
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// The collision avoidance behaviour is used to steer away from moving 
/// obstacles, not so much static geometry.
/// </summary>
class CollisionAvoidanceSteer : ISteeringBehaviour
{

	public float LookAhead = 4.0f;
	public float Radius = 1.0f;
	public float MaxAcceleration = 8.0f;

	public CollisionAvoidanceSteer()
	{
	}

	virtual public SteeringOutput CalculateAcceleration(Agent agent)
	{
		SteeringOutput output = new SteeringOutput();

		KinematicInfo info = agent.KinematicInfo;

		output.Angular = 0.0f;
		output.Linear = Vector2.zero;

		KinematicInfo firstTarget = null;
		float firstMinSeperation = 0.0f;
		float firstDistance = 0.0f;
		float shortestTime = Mathf.Infinity;
		Vector2 firstRelativePos = Vector2.zero;
		Vector2 firstRelativeVel = Vector2.zero;

		var agentList = agent.GetAgentsInArea(LookAhead);

		Vector2 forward = MotionUtils.GetOrientationAsVector(info.Orientation);


		foreach (var a in agentList) {
			// Check whether we are facing the agent.
			if (a == agent) {
				continue;
			}

			// Check whether we are facing the agent.
			Vector2 relativePos = a.KinematicInfo.Position - info.Position;

			// Okay, time to apply our stuff to it.
			Vector2 relativeVelocity = a.KinematicInfo.Velocity - info.Velocity;
			float relativeSpeed = relativeVelocity.magnitude;
			float timeToCollision = Vector2.Dot(relativePos, -relativeVelocity) /
				(relativeSpeed * relativeSpeed);

			float distance = relativePos.magnitude;
			float minSeperation = distance - relativeSpeed * shortestTime;
			if (minSeperation > 2 * Radius) {
				continue;
			}
            
			if (timeToCollision > 0.0f && timeToCollision < shortestTime) {
				shortestTime = timeToCollision;
				firstTarget = a.KinematicInfo;
				firstMinSeperation = minSeperation;
				firstDistance = distance;
				firstRelativePos = relativePos;
				firstRelativeVel = relativeVelocity;
			}

		}

		if (firstTarget == null) {
			return output;
		}

		Vector2 relative = Vector2.zero;
		if (firstMinSeperation <= 0 || firstDistance < 2 * Radius) {
			relative = firstTarget.Position - info.Position;
		} else {
			relative = firstRelativePos + firstRelativeVel * shortestTime;
		}

		relative.Normalize();

		output.Linear -= relative * MaxAcceleration;
		return output;
        
	}
}
