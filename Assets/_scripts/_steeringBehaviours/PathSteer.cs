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
	public float MaxAcceleration;
	public KinematicInfo Target = new KinematicInfo();
	
	public Vector2 LocalTarget {
		get;
		private set;
	}
	
	public PathSteer()
	{
	}

	virtual public SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;
	
		AStarUtils.GetPath(info.Position, Target.Position, OnPathCalculated);
	
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
