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
					MotionUtils.Vec3ToVec2(w.transform.position)) < radius)
			.Select(p => p.transform.position)
			.ToList();
//		Debug.Log(nearObstacles.Count());
		if(nearObstacles == null || nearObstacles.Count == 0) return steeringOutput;
		
		Vector2 direction = (MotionUtils.Vec3ToVec2(nearObstacles[0]) - info.Position).normalized;
		for(int i = 1; i < nearObstacles.Count; ++i){
			var nextDir = (MotionUtils.Vec3ToVec2(nearObstacles[i]) - info.Position).normalized;
			direction += nextDir;
		}
		direction /= nearObstacles.Count;
		direction.Normalize();
		
		steeringOutput.Linear = (-direction) * MaxAcceleration;
		return steeringOutput;
	}
}
