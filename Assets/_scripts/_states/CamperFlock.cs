using System;
using System.Linq;
using UnityEngine;

public class CamperFlock : AgentState
{
	private WanderSteer wander = new WanderSteer();
	private CollisionAvoidanceSteer avoid = new CollisionAvoidanceSteer();
	private SeperationSteer seperation = new SeperationSteer();
	private CohesionSteer cohesionSteer = new CohesionSteer();

	public void InitAction()
	{
		Debug.Log("init CamperFlock state");
		agent.AddBehaviour("wander", wander, 2);
		agent.AddBehaviour("avoid", avoid, 0);
		agent.AddBehaviour("seperation", seperation, 0);
		agent.AddBehaviour("cohesion", cohesionSteer, 1);

		//agent.AddBehaviour("look", new LWYGSteer(), 0); 

		agent.MaxVelocity = 4.0f;  
		agent.MaxAcceleration = 4.0f;
		agent.MaxAngularVelocity = 2 * Mathf.PI; 

		wander.WanderOrientation = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
	}

	public void ExitAction()
	{
		//TODO: remove all relevant behaviours 
		agent.RemoveBehaviour("wander");

	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();

		int numAgents = agent.GetAgentsInArea(Config.DefaultCamperFlockRadius).
			Where(a => a is Camper).Count(); 
		
		if (numAgents == 0) {
			/// camper has no company -> Idle
			nextState = typeof(CamperIdle);
		}

	}
}


