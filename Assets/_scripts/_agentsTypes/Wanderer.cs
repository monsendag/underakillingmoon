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
        agent.MaxVelocity = 9.0f;
        agent.MaxAcceleration = 27.0f;
        agent.MaxAngularVelocity = Mathf.PI / 100;

        WanderSteer wanderSteer = new WanderSteer();

        wanderSteer.MaxAcceleration = 16.0f;
        wanderSteer.MaxAngularAcceleration = Mathf.PI / 8;
        wanderSteer.TargetRadius = 0.025f;
        wanderSteer.SlowRadius = 0.05f;
        wanderSteer.MaxOrientationChange = Mathf.PI / 18; // 10 degrees
        wanderSteer.WanderOffset = 8.0f;
        wanderSteer.WanderRadius = 0.5f;

        ArriveSteer arriveSteer = new ArriveSteer();
        SeekSteer seekSteer = new SeekSteer();

        seekSteer.MaxAcceleration = 16.0f;

        arriveSteer.MaxVelocity = 8.0f;
        arriveSteer.TargetRadius = 0.2f;
        arriveSteer.MaxAcceleration = 16.0f;
        arriveSteer.SlowRadius = 12.0f;
        arriveSteer.TimeToTarget = 0.1f;
        target.x = Random.Range(-48.0f, 48.0f);
        target.y = Random.Range(-48.0f, 48.0f);

        arriveSteer.Target.Position = target;
        seekSteer.Target.Position = target;

        //agent.AddBehaviour("arrive", arriveSteer, 0);

        //agent.AddBehaviour("LWYG",new LWYGSteer(),0);
        agent.AddBehaviour("wander", wanderSteer, 0);
        //agent.AddBehaviour("seek", seekSteer, 0);
        
	}
	
	// Update is called once per frame
	void Update() 
    {
        Vector2 dif = agent.KinematicInfo.Position - target;
        if (dif.magnitude < 1.0f)
        {
            target.x = Random.Range(-48.0f, 48.0f);
            target.y = Random.Range(-48.0f, 48.0f);
         
  //          ArriveSteer arriveSteer = (ArriveSteer) agent.GetBehaviour("arrive");
  //          arriveSteer.Target.Position = target;
        }
	}

    void OnDrawGizmos()
    {
        Vector3 targetActual = new Vector3(target.x, 0.0f, target.y);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetActual, 1.0f);
    }
}
