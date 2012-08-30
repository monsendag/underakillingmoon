/**
 * Implements the seek behaviour
 **/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using Pathfinding;

/// <summary>
/// The PathSteer behaviour creates the effect of a unit seeking a target.
/// </summary>
public class PathSteer : SeekSteer
{
	public int TimeBetweenPathUpdate = 25;
	public Vector2 LocalTarget;
	
	int _nextPathUpdate = 0;
	
	public PathSteer() : base()
	{
		_nextPathUpdate = System.Environment.TickCount;
	}

	override public SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;
	
		if(Time.time > _nextPathUpdate){
			AStarUtils.GetPath(info.Position, LocalTarget, OnPathCalculated);
			_nextPathUpdate = System.Environment.TickCount + TimeBetweenPathUpdate;
		}
	
		return base.CalculateAcceleration(agent);
	}
	
	void OnPathCalculated(Path p)
	{
		List<Vector2> path = AStarUtils.FilterPathAsList(p.vectorPath);
		var _movementDirection = (path.Count > 1)?(path[1] - path[0]).normalized : Vector2.zero;
		Target.Position = _movementDirection;
	}
}
