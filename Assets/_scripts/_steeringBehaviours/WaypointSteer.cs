using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointSteer : PathSteer {
	public float ArriveDistance = 1.0f;
	ShuffleBag<Vector2> _shuffleBag = new ShuffleBag<Vector2>(), _fullBag = new ShuffleBag<Vector2>();
	
	public ShuffleBag<Vector2> Waypoints
	{
		get {
			return _shuffleBag;
		}
		set{ //set _waypoints and reset pathfinding
			_shuffleBag = value;
			_fullBag.Copy(_shuffleBag);
			if(_shuffleBag.Bag.Count != 0)LocalTarget = _shuffleBag.PopShuffleBagItem();
		}
	}

	public WaypointSteer() : base(){}
	
	override public SteeringOutput CalculateAcceleration (Agent agent)
	{
		var info = agent.KinematicInfo;
		//if close enough to the waypoint (defined by ArriveDistance)
		//move on to next one
		if(Vector2.Distance(info.Position, Target.Position) < ArriveDistance){
			if((LocalTarget = _shuffleBag.PopShuffleBagItem()) == null){
				_shuffleBag.Copy(_fullBag);
				LocalTarget = _shuffleBag.PopShuffleBagItem();
			}
			Target.Position = LocalTarget;
		}
		return base.CalculateAcceleration (agent);
	}
	
	
}
