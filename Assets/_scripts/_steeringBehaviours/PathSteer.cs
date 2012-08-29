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
public class PathSteer : ISteeringBehaviour
{
	public float MaxAcceleration = 1.0f;
	public float TimeBetweenPathUpdate = 0.25f;
	public KinematicInfo Target = new KinematicInfo();
	
	Vector2 _movementDirection;
	
	float _nextPathUpdate = 0.0f;
	
	public PathSteer()
	{
		_nextPathUpdate = Time.time;
	}

	virtual public SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;
	
		if(Time.time > _nextPathUpdate){
			AStarUtils.GetPath(info.Position, Target.Position, OnPathCalculated);
			_nextPathUpdate = Time.time + TimeBetweenPathUpdate;
		}
	
		SteeringOutput steering = new SteeringOutput();
		steering.Linear = _movementDirection * MaxAcceleration;
		return steering;
	}
	
	void OnPathCalculated(Path p)
	{
		List<Vector2> path = AStarUtils.FilterPathAsList(p.vectorPath);
		_movementDirection = (path.Count > 1)?(path[1] - path[0]).normalized : Vector2.zero;
	}
}
