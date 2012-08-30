using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointSteer : PathSteer {
	public float ArriveDistance = 1.0f;
	int _currentWaypoint = 0;
	List<Vector2>  _waypoints;
	
	public List<Vector2> Waypoints{
		get {
			return _waypoints;
		}
		set{
			_waypoints = value;
			_currentWaypoint = 0;
			if(value != null)LocalTarget = _waypoints[0];
		}
	}

	public WaypointSteer() : base(){}
	
	override public SteeringOutput CalculateAcceleration (Agent agent)
	{
		var info = agent.KinematicInfo;
		if(Vector2.Distance(info.Position, Target.Position) < ArriveDistance){
			_currentWaypoint = (_currentWaypoint < Waypoints.Count - 1)? _currentWaypoint + 1 : 0;
			Target.Position = Waypoints[_currentWaypoint];
		}
		return base.CalculateAcceleration (agent);
	}
}
