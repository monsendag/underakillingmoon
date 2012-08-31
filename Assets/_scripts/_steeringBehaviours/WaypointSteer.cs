using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointSteer : PathSteer {
	public float ArriveDistance = 1.0f;
	List<Vector2>  _waypoints, _shuffleBag = new List<Vector2>();
	
	public List<Vector2> Waypoints
	{
		get {
			return _waypoints;
		}
		set{ //set _waypoints and reset pathfinding
			_waypoints = value;
			createShuffleBag();
			if(_shuffleBag.Count != 0)LocalTarget = popShuffleBag();
		}
	}

	public WaypointSteer() : base(){}
	
	override public SteeringOutput CalculateAcceleration (Agent agent)
	{
		var info = agent.KinematicInfo;
		//if close enough to the waypoint (defined by ArriveDistance)
		//move on to next one
		if(Vector2.Distance(info.Position, Target.Position) < ArriveDistance){
			Target.Position = popShuffleBag();
		}
		return base.CalculateAcceleration (agent);
	}
	
	void createShuffleBag(){
		_shuffleBag.Clear();
		
		if(_waypoints == null || _waypoints.Count == 0)
			return;
		
		for(int i = 0; i < _waypoints.Count; ++i){
			for(int j = i + 1; j > 0; --j){
				_shuffleBag.Add(_waypoints[i]);	
			}
		}
	}
	
	Vector2 popShuffleBag(){
		if(_shuffleBag.Count == 0)
			return Vector2.zero;
		
		int random = UnityEngine.Random.Range(0, _shuffleBag.Count);
		Vector2 randomPoint = _shuffleBag[random];
		_shuffleBag.RemoveAt(random);
		
		if(_shuffleBag.Count == 0)
			createShuffleBag();
		
		return randomPoint;
	}
}
