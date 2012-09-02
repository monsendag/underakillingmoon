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
public class PathSteer : SeekSteer
{
	public float TimeBetweenPathUpdate = 0.25f;
	public Vector2 LocalTarget;
    private List<Vector2> points = null;

    float _timeSinceUpdate = 0;
	
	public PathSteer() : base()
	{
	}

	override public SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;
		//after time increment, update path and set next update
        _timeSinceUpdate += Time.deltaTime ;
		if(_timeSinceUpdate >= TimeBetweenPathUpdate){
            _timeSinceUpdate -= TimeBetweenPathUpdate;
			AStarUtils.GetPath(info.Position, LocalTarget, OnPathCalculated);
            //Debug.Break();
		}
        // Draw the path.
        if (points != null)
        {
            Vector2 prevPoint = MotionUtils.To2D(agent.transform.position);
            foreach (var point in points)
            {
                Debug.DrawLine(MotionUtils.To3D(prevPoint), MotionUtils.To3D(point),
                   Color.magenta);
                prevPoint = point;
            }
        }
	
		return base.CalculateAcceleration(agent);
	}
	
	void OnPathCalculated(Path p)
	{
		//get direction from path and update target position
		List<Vector2> path = AStarUtils.PathToList(p.vectorPath);
		var _movementDirection = (path.Count > 1)?(path[1] - path[0]).normalized : Vector2.zero;
        points = path;
		Target.Position = path[1];
	}
}
