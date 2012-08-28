using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Agent))]
public class Pathfinder : MonoBehaviour
{
	public Transform target;
	Agent _agent;
	PathSteer _pathSteer;
	

	// Use this for initialization
	void Start ()
	{
		Time.timeScale = 0.5f;
		_agent = gameObject.GetComponent<Agent>();
		DebugUtil.Assert (_agent != null);
		_agent.MaxVelocity = 9.0f;
		_agent.MaxAcceleration = 27.0f;
		_agent.MaxAngularVelocity = Mathf.PI / 100;
		
		_pathSteer = new PathSteer ();
		_pathSteer.MaxAcceleration = 16.0f;
		_pathSteer.Target.Position = new Vector2(target.position.x, target.position.z);
		
		ObstacleAvoidanceSteer oaSteer = new ObstacleAvoidanceSteer();
		oaSteer.MaxAcceleration = 16.0f;
		
		List<Vector2> waypoints = GameObject.FindGameObjectsWithTag("Campfire")
			.Select(w => new Vector2(w.transform.position.x, w.transform.position.z))
			.ToList();
		Debug.Log(waypoints.Count());
		WaypointSteer waypointSteer = new WaypointSteer(waypoints);
		waypointSteer.MaxAcceleration = 16.0f;
		
//		_agent.AddBehaviour("waypoint", waypointSteer, 0);
		_agent.AddBehaviour("path", _pathSteer, 0);
		_agent.AddBehaviour("obstacleAvoidance", oaSteer, 0);
	}
}
