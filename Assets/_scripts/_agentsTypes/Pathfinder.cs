using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Agent))]
public class Pathfinder : MonoBehaviour
{
	public Transform target;
	Agent _agent;
	PathSteer _pathSteer = new PathSteer ();
	WaypointSteer _waypointSteer = new WaypointSteer();
	ObstacleAvoidSteer _oaSteer = new ObstacleAvoidSteer();
	LWYGSteer _look = new LWYGSteer();

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Hello!");
		_agent = gameObject.GetComponent<Agent>();
		DebugUtil.Assert (_agent != null);
		_agent.MaxVelocity = 5.0f;
		_agent.MaxAcceleration = 8.0f;
		_agent.MaxAngularVelocity = Mathf.PI / 100;
		
		_pathSteer.MaxAcceleration = 4.0f;
		_pathSteer.Target.Position = new Vector2(target.position.x, target.position.z);
		
		_oaSteer.MaxAcceleration = 8.0f;
		
		_look.TimeToTarget = 1.0f;
		
		List<Vector2> waypoints = GameObject.FindGameObjectsWithTag("Campfire")
			.Select(w => MotionUtils.Vec3ToVec2(w.transform.position))
			.OrderBy(w => Vector2.Distance(_agent.KinematicInfo.Position, w))
			.ToList();
		_waypointSteer.Waypoints = waypoints;
		_waypointSteer.MaxAcceleration = 4.0f;
		
		_agent.AddBehaviour("waypoint", _waypointSteer, 0);
//		_agent.AddBehaviour("path", _pathSteer, 0);
		
//		_agent.AddBehaviour("look", _look, 0);
		_agent.AddBehaviour("obstacleAvoidance", _oaSteer, 0);
	}
	
	void OnDrawGizmos(){
		if(!Application.isPlaying) return;
		/*Gizmos.DrawLine(MotionUtils.Vec2ToVec3(_agent.KinematicInfo.Position), 
			MotionUtils.Vec2ToVec3(_agent.KinematicInfo.Position + _oaSteer.Direction));*/
	}
}
