using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObstacleAvoidanceSteer : ISteeringBehaviour {
	public float radius = 2.50f;
	public float MaxAcceleration;
	
	public ObstacleAvoidanceSteer(){}
	
	public SteeringOutput CalculateAcceleration (Agent agent)
	{
		var info = agent.KinematicInfo;
		SteeringOutput steeringOutput = new SteeringOutput();
		List<Vector3> nearObstacles = (List<Vector3>)GameObject.FindGameObjectsWithTag("Obstacle")
			.Where (w => Vector2.Distance(info.Position, 
					new Vector2(w.transform.position.x, w.transform.position.z)) < radius)
			.Select(p => p.transform.position)
			.ToList();
//		Debug.Log(nearObstacles.Count());
		if(nearObstacles == null || nearObstacles.Count == 0) return steeringOutput;
		
		Vector2 direction = (new Vector2(nearObstacles[0].x, nearObstacles[0].z) - info.Position).normalized;
		for(int i = 1; i < nearObstacles.Count; ++i){
			var nextDir = (new Vector2(nearObstacles[i].x, nearObstacles[i].z) - info.Position).normalized;
			direction += nextDir;
		}
		direction /= nearObstacles.Count;
		direction.Normalize();
		
		steeringOutput.Linear = (-direction.normalized) * MaxAcceleration;
		return steeringOutput;
	}
}
