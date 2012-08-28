using System;
using System.Linq;
using UnityEngine;

public class CamperFlock : AgentState
{
	private WanderSteer _wander = new WanderSteer();
	private CollisionAvoidanceSteer _avoid = new CollisionAvoidanceSteer();
	private SeperationSteer _seperation = new SeperationSteer();
	private CohesionSteer _cohesionSteer = new CohesionSteer();
    private LWYGSteer _look = new LWYGSteer();
    private FrictionSteer _friction = new FrictionSteer();

	public void InitAction()
	{
		Debug.Log("init CamperFlock state");

        _look.TimeToTarget = 1.0f;
        _friction.VelocityFrictionPercentage = 1.5f;
        _avoid.LookAhead = 1.5f;
        _seperation.Threshold = 1.5f;
        _seperation.DecayCoefficient = 3.0f;
        _cohesionSteer.MaxAcceleration = 5.0f;
        _cohesionSteer.PlayerWeight = 24;

		agent.AddBehaviour("wander", _wander, 2);
		agent.AddBehaviour("avoid", _avoid, 1);
		agent.AddBehaviour("seperation", _seperation, 0);
		agent.AddBehaviour("cohesion", _cohesionSteer, 1);
        agent.AddBehaviour("friction", _friction, 1);
		agent.AddBehaviour("look", _look, 0); 

		agent.MaxVelocity = 4.0f;  
		agent.MaxAcceleration = 4.0f;
		agent.MaxAngularVelocity = 2 * Mathf.PI; 

		_wander.WanderOrientation = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
	}

	public void ExitAction()
	{
		//TODO: remove all relevant behaviours 
		agent.RemoveBehaviour("wander");
		agent.RemoveBehaviour("cohesion");
        agent.RemoveBehaviour("avoid");
        agent.RemoveBehaviour("seperation");
        agent.RemoveBehaviour("look");
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


