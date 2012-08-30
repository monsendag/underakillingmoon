using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WerewolfPatrol : AgentState
{
	Agent target;
	WaypointSteer _waypointSteer;
    LWYGSteer _look = new LWYGSteer();

	public void InitAction()
	{
		target = null;
		AttackPair.RemoveByAttacker(agent);
		
		List<Vector2> waypoints = GameObject.FindGameObjectsWithTag("Campfire")
			.Select(w => new Vector2(w.transform.position.x, w.transform.position.z))
			.OrderBy(w => Vector2.Distance(agent.KinematicInfo.Position, w))
			.ToList();
		if(waypoints != null){
			_waypointSteer = new WaypointSteer(waypoints);
			_waypointSteer.MaxAcceleration = 16.0f;
			agent.AddBehaviour("waypoint", _waypointSteer, 0);
            agent.AddBehaviour("look", _look, 0);
		}
	}

	public void ExitAction()
	{
        if (_waypointSteer != null)
        {
		   agent.RemoveBehaviour("waypoint");
        }
        agent.RemoveBehaviour("look");
	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();

		// search for nearby campers


        var target = AttackPair.GetTargetOrNull(agent);
		// Found a target -> Charge towards it
		if (target != null) {
			nextState = typeof(WerewolfCharge);
		}
	}
}

