using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WerewolfPatrol : AgentState
{
	Agent target;
	WaypointSteer _waypointSteer = new WaypointSteer();
	LWYGSteer _look = new LWYGSteer();
	
	public void InitAction()
	{
		target = null;
		AttackPair.RemoveByAttacker(agent);
		
		//Linq queries will grab all game objects with the tag "Campfire"
		//order them by distance from the player
		//and return the position as a Vector2
		List<Vector2> waypoints = GameObject.FindGameObjectsWithTag("Campfire")
			.Select(w => MotionUtils.To2D(w.transform.position))
			.OrderBy(w => Vector2.Distance(agent.KinematicInfo.Position, w))
			.ToList();
		if(waypoints != null){
			_waypointSteer.Waypoints = waypoints;
			_waypointSteer.MaxAcceleration = 16.0f;
			agent.AddBehaviour("waypoint", _waypointSteer, 0);
		}
		
		agent.AddBehaviour("look", _look, 0);
	}

	public void ExitAction()
	{
		if(_waypointSteer != null)
			agent.RemoveBehaviour("waypoint");
		 agent.RemoveBehaviour("look");
	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();

		// search for nearby campers
		target = agent.GetAgentsInArea(Config.DefaultWerewolfVisionRange) 
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

