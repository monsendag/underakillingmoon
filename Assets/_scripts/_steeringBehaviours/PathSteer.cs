/**
 * Implements the seek behaviour
 **/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

/// <summary>
/// The PathSteer behaviour creates the effect of a unit seeking a target.
/// </summary>
public class PathSteer : ISteeringBehaviour
{
	public float MaxAcceleration = 1.0f;
	public float TimeBetweenPathUpdate = 0.25f;
	public KinematicInfo Target = new KinematicInfo();
	
	protected Vector2 LocalTarget;
	
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
		steering.Linear = LocalTarget - info.Position;
		steering.Linear.Normalize();
		steering.Linear *= MaxAcceleration;
		return steering;
	}
	
	void OnPathCalculated(Path p)
	{
		var path = AStarUtils.FilterPath(p.vectorPath) [1];
		LocalTarget = new Vector2(path.x, path.z);
	}
}
