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
    private ObstacleAvoidSteer _obstacleAvoid = new ObstacleAvoidSteer();

	public void InitAction()
	{
		Debug.Log("init CamperFlock state");

        _look.TimeToTarget = 1.0f;
        _friction.VelocityFrictionPercentage = 0.0f;
        _avoid.LookAhead = 1.0f;
        _seperation.Threshold = 1.0f;
        _seperation.DecayCoefficient = 3.0f;
        _cohesionSteer.MaxAcceleration = 15.0f;
        _cohesionSteer.PlayerWeight = 30;
        _cohesionSteer.Radius = 12.0f;
        _friction.AngularVelocityFrictionPercentage = 0.5f;

		agent.AddBehaviour("wander", _wander, 3);
		agent.AddBehaviour("avoid", _avoid, 1);
		agent.AddBehaviour("seperation", _seperation, 2);
		agent.AddBehaviour("cohesion", _cohesionSteer, 2);
        agent.AddBehaviour("friction", _friction, 2);
		agent.AddBehaviour("look", _look, 0);
        agent.AddBehaviour("obstacleAvoid", _obstacleAvoid, 1);

		agent.MaxVelocity = 4.0f;  
		agent.MaxAcceleration = 4.0f;
		agent.MaxAngularVelocity = 2 * Mathf.PI; 

		_wander.WanderOrientation = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
	}

	public void ExitAction()
	{
		//TODO: remove all relevant behaviours 
		//agent.RemoveBehaviour("wander");
		agent.RemoveBehaviour("cohesion");
        agent.RemoveBehaviour("avoid");
        agent.RemoveBehaviour("seperation");
        agent.RemoveBehaviour("look");
        agent.RemoveBehaviour("friction");
        agent.RemoveBehaviour("obstacleAvoid");
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


