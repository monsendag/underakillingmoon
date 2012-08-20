using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CamperFlock : AgentStateMachine
{
	private WanderSteer wander = new WanderSteer();
	private CollisionAvoidanceSteer avoid = new CollisionAvoidanceSteer();
	private SeperationSteer seperation = new SeperationSteer();
	private CohesionSteer cohesionSteer = new CohesionSteer();

	public void InitAction()
	{

		Debug.Log("init CamperFlock state");
		agent.ClearBehaviours();
		agent.AddBehaviour("wander", wander, 2);
		agent.AddBehaviour("avoid", avoid, 0);
		agent.AddBehaviour("seperation", seperation, 0);
		agent.AddBehaviour("cohesion", cohesionSteer, 1);

		//agent.AddBehaviour("look", new LWYGSteer(), 0); 

		agent.MaxVelocity = 4.0f;  
		agent.MaxAcceleration = 4.0f;
		agent.MaxAngularVelocity = 2 * Mathf.PI; 

		wander.WanderOrientation = Random.Range(-Mathf.PI, Mathf.PI);
	}

	public void ExitAction()
	{

	}
	
	public override void Update(out AgentStateMachine nextState)
	{
		nextState = this;
	
		int numAgents = agent.GetAgentsInArea(Config.CamperFlockRadius).
			Where(a => a is Camper).Count(); 
		
		if (numAgents == 0) {
			/// camper has no company -> Idle
			nextState = new CamperIdle();
		}

	}
}


