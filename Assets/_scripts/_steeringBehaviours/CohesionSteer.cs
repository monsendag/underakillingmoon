/**
 * Collision Avoidance steer
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// Try to keep agents from drifting too far apart by moving them towards the 
/// center of the group.
/// </summary>
class CohesionSteer : PathSteer
{

	public float LookAhead = 9.0f;
    public float MinLookAhead = 1.5f;
	public float Radius = 1.0f;
	public float MaxAcceleration = 12.0f;
    public bool FollowPlayer = true;
    public int PlayerWeight = 18;

    private Vector2 _lastKnownPlayerPosition = Vector2.zero;
    private bool _spottedPlayer = false;

	public CohesionSteer()
	{
        _spottedPlayer = false;
        _lastKnownPlayerPosition = Vector2.zero;
	}

	override public SteeringOutput CalculateAcceleration(Agent agent)
	{
		SteeringOutput output = new SteeringOutput();
		KinematicInfo info = agent.KinematicInfo;

		output.Angular = 0.0f;
		output.Linear = Vector2.zero;

		var agentList = agent.GetAgentsInArea(LookAhead);

		Vector2 average = new Vector2();
		Vector2 velocityAverage = new Vector2();
		int num = 0;

		// Get the average pack location
		foreach (var a in agentList) {
			// Check whether we are facing the agent.
			if (a == agent) {
				continue;
			}

            if (a.GetComponent<PlayerMovement>() != null && FollowPlayer)
            {
                _spottedPlayer = true;
                _lastKnownPlayerPosition = a.KinematicInfo.Position;
                num += PlayerWeight;
                average += PlayerWeight * a.KinematicInfo.Position;
                velocityAverage += PlayerWeight * a.KinematicInfo.Velocity;
            }
            else if (a.GetComponent<Werewolf>() == null) // Make sure we don't flock with werewolves.
            {
                num++;
                average += a.KinematicInfo.Position;
                velocityAverage += a.KinematicInfo.Velocity;
            }
		}

		if (num == 0) {
			return output;
		}


		average /= num;
		velocityAverage /= num;

		Vector2 positionDif = average - info.Position;
		Vector2 velocityDif = velocityAverage - info.Velocity;
		if (velocityDif.magnitude > MaxAcceleration) {
			velocityDif = velocityDif.normalized * MaxAcceleration;
		}

        if (positionDif.magnitude < MinLookAhead)
        {
            return output;
        }

		float strength = (positionDif.magnitude - MinLookAhead) / 2 * (LookAhead - MinLookAhead);

		output.Linear += (positionDif.normalized) * MaxAcceleration;
		//output.Linear += velocityDif / 2.0f;

        // Check to see if our path is blocked by obstacles. 
        if (_spottedPlayer)
        {
            _lastKnownPlayerPosition =
                GameObject.FindGameObjectWithTag("Player").GetComponent<Agent>().KinematicInfo.Position;

            Vector3 motion = (MotionUtils.To3D(_lastKnownPlayerPosition) -
                agent.transform.position);
            var hits = Physics.RaycastAll(
                agent.transform.position, motion.normalized, motion.magnitude);

            bool reachable = true;

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag == "Obstacle")
                {
                    reachable = false;
                }
            }
            if (!reachable)
            {
                // We can't reach the target position. Let's delegate to the pathfinding.
                base.LocalTarget = _lastKnownPlayerPosition;
                base.UpdatePath();

                output = base.CalculateAcceleration(agent);
                output.Linear.Normalize();
                output.Linear *= MaxAcceleration;
            }
        }

		return output;

	}

}