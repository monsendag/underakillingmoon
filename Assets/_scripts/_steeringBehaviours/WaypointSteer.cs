using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaypointSteer : PathSteer {
	public float ArriveDistance = 2.5f;
	IndexShuffleBag _shuffleBag = new IndexShuffleBag(), _fullBag = new IndexShuffleBag();
	List<Vector2> _waypoints;
	
	public IndexShuffleBag IndexBag{
		get{
			return _shuffleBag;
		}
		set{
			_shuffleBag.Copy(value);
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
			if(_shuffleBag.Bag.Count != 0)
				LocalTarget = _waypoints[_shuffleBag.PopShuffleBagItem()];
		}
	}
	

	public WaypointSteer() : base(){}
	
	override public SteeringOutput CalculateAcceleration (Agent agent)
	{
		var info = agent.KinematicInfo;
		//if close enough to the waypoint (defined by ArriveDistance)
		//move on to next one
		if(Vector2.Distance(info.Position, LocalTarget) < ArriveDistance){
			if(_shuffleBag.Bag.Count == 0) _shuffleBag.Copy(_fullBag);
			
			_waypoints = _waypoints.OrderBy(w => Vector2.Distance(w, info.Position)).ToList();
			LocalTarget = _waypoints[_shuffleBag.PopShuffleBagItem()];
			Target.Position = LocalTarget;
		}
		return base.CalculateAcceleration (agent);
	}
	
	
}
