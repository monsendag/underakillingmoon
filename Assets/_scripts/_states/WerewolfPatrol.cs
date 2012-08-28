using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WerewolfPatrol : AgentState
{
	Agent target;
	WaypointSteer waypointSteer;

	public void InitAction()
	{
		target = null;
		AttackPair.RemoveByAttacker(agent);
		
		List<Vector2> waypoints = GameObject.FindGameObjectsWithTag("Campfire")
			.Select(w => new Vector2(w.transform.position.x, w.transform.position.z))
			.ToList();
		if(waypoints != null){
			waypointSteer = new WaypointSteer(waypoints);
			waypointSteer.MaxAcceleration = 16.0f;
			agent.AddBehaviour("waypoint", waypointSteer, 0);
		}
	}

	public void ExitAction()
	{
		if(waypointSteer != null)
			agent.RemoveBehaviour("waypoint");
	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();

		// search for nearby campers
		target = agent.GetAgentsInArea(Config.DefaultWerewolfVIsionRange) 
			.Where(c => c is Camper) // we only like Camper meat
			.OrderBy(a => agent.distanceTo(a)) // order by distance
			.FirstOrDefault(); // select closest

		// Found a target -> Charge towards it
		if (target != null) {
			AttackPair.Add(agent, target);
			nextState = typeof(WerewolfCharge);
		}
	}
}

