using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointSteer : PathSteer {
	public float ArriveDistance = 1.0f;
	IndexShuffleBag _shuffleBag, _fullBag = new IndexShuffleBag();
	List<Vector2> _waypoints;
	
	public IndexShuffleBag IndexBag{
		get{
			return _shuffleBag;
		}
		set{
			_shuffleBag = value;
			_fullBag.Copy(_shuffleBag);
		}
	}
	
	public List<Vector2> Waypoints
	{
		get {
			return _waypoints;
		}
		set{ //set _waypoints and reset pathfinding
			_waypoints = value;
			if(!(_shuffleBag == null || _shuffleBag.Bag.Count == 0))
				LocalTarget = _waypoints[_shuffleBag.PopShuffleBagItem()];
		}
	}
	

	public WaypointSteer() : base(){}
	
	override public SteeringOutput CalculateAcceleration (Agent agent)
	{
		var info = agent.KinematicInfo;
		//if close enough to the waypoint (defined by ArriveDistance)
		//move on to next one
		if(Vector2.Distance(info.Position, Target.Position) < ArriveDistance){
			if(_shuffleBag.Bag.Count == 0) _shuffleBag.Copy(_fullBag);
			LocalTarget = _waypoints[_shuffleBag.PopShuffleBagItem()];
			Target.Position = LocalTarget;
		}
		return base.CalculateAcceleration (agent);
	}
	
	
}
