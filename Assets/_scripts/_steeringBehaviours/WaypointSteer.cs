using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointSteer : PathSteer {
	public float ArriveDistance = 1.0f;
	
	List<Vector2> _waypoints;
	int _currentWaypoint = 0;
	
	public WaypointSteer(List<Vector2> _wps) : base(){
		_waypoints = _wps;
		_currentWaypoint = 0;
		Target.Position = _waypoints[0];
	}
	
	public override SteeringOutput CalculateAcceleration (Agent agent)
	{
		var info = agent.KinematicInfo;
		if(Vector2.Distance(info.Position, LocalTarget) < ArriveDistance){
			_currentWaypoint = (_currentWaypoint < _waypoints.Count - 1)? _currentWaypoint + 1 : 0;
			Target.Position = _waypoints[_currentWaypoint];
		}
		return base.CalculateAcceleration (agent);
	}
}
