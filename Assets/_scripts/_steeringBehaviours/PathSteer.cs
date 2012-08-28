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
public class PathSteer : ArriveSteer
{
	/*public float MaxAcceleration;*/
	public KinematicInfo Target = new KinematicInfo();
	
	public Vector2 LocalTarget {
		get;
		private set;
	}
	
	public PathSteer() : base()
	{
        base.SlowRadius = 6.0f;
        base.TargetRadius = 1.0f;
        base.TimeToTarget = 0.5f;
        base.MaxVelocity = 8.0f;
	}

	override public SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;
	
		AStarUtils.GetPath(info.Position, Target.Position, OnPathCalculated);

        base.Target.Position = LocalTarget;
		return base.CalculateAcceleration(agent);
	}
	
	void OnPathCalculated(Path p)
	{
		var path = AStarUtils.FilterPath(p.vectorPath) [2];
		LocalTarget = new Vector2(path.x, path.z);
        base.Target.Position = LocalTarget;
	}
}
