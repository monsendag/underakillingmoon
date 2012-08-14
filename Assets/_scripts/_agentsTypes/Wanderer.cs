/**
 * The wanderer agent is a sophisticated way of steering a capsule around
 * aimlessly.
 **/
using UnityEngine;
using System.Collections;

public class Wanderer : Agent
{
	Vector2 target;

	// Use this for initialization
	public override void Start()
	{
		base.Start();
		// And set it up with a wandering state.
		MaxVelocity = 4.0f;
		MaxAcceleration = 4.0f;
		MaxAngularVelocity = 2 * Mathf.PI;

		WanderSteer wanderSteer = new WanderSteer();

		wanderSteer.MaxAcceleration = 4.0f; 
		wanderSteer.TargetRadius = 0.025f;
		wanderSteer.SlowRadius = 0.5f;
		wanderSteer.MaxOrientationChange = Mathf.PI / 4; // 45 degrees
		wanderSteer.WanderOffset = 2.9f;
		wanderSteer.WanderRadius = 0.75f;

		ArriveSteer arriveSteer = new ArriveSteer();
		SeekSteer seekSteer = new SeekSteer();

		seekSteer.MaxAcceleration = 2.0f;

		arriveSteer.MaxVelocity = 2.0f;
		arriveSteer.TargetRadius = 0.2f;
		arriveSteer.MaxAcceleration = 2.0f;
		arriveSteer.SlowRadius = 1.5f;
		arriveSteer.TimeToTarget = 1.0f;
		target.x = Random.Range(-5.0f, 5.0f);
		target.y = Random.Range(-5.0f, 5.0f);

		arriveSteer.Target.Position = target;
		seekSteer.Target.Position = target;

		//AddBehaviour("arrive", arriveSteer, 0);

		//AddBehaviour("LWYG",new LWYGSteer(),0);
		AddBehaviour("wander", wanderSteer, 0);
		//AddBehaviour("seek", seekSteer, 0);
        
	}

	public override void Update()
	{
		base.Update();
	}

}
