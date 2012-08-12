/**
 * The wanderer agent is a sophisticated way of steering a capsule around
 * aimlessly.
 **/
using UnityEngine;
using System.Collections;

public class Wanderer : MonoBehaviour 
{
    Vector2 target;
    Agent agent;

	// Use this for initialization
	void Start () 
    {
        // Add an agent component.
        agent = gameObject.AddComponent<Agent>();
        DebugUtil.Assert(agent != null);
        // And set it up with a wandering state.
        agent.MaxVelocity = 4.0f;
        agent.MaxAcceleration = 4.0f;
        agent.MaxAngularVelocity = 2 * Mathf.PI;

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

        //agent.AddBehaviour("arrive", arriveSteer, 0);

        //agent.AddBehaviour("LWYG",new LWYGSteer(),0);
        agent.AddBehaviour("wander", wanderSteer, 0);
        //agent.AddBehaviour("seek", seekSteer, 0);
        
	}

}
