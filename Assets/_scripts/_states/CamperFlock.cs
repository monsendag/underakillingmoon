using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CamperFlock : IAgentState
{
    private bool initialised = false;

    private WanderSteer wander = new WanderSteer();
    private CollisionAvoidanceSteer avoid = new CollisionAvoidanceSteer();
    private SeperationSteer seperation = new SeperationSteer();
    private CohesionSteer cohesionSteer = new CohesionSteer();

	public CamperFlock ()
	{
	}

    private void Initialise(Agent agent)
    {
        if (initialised)
        {
            return;
        }
        initialised = true;


        agent.ClearBehaviours();
        agent.AddBehaviour("wander",wander,2);
        agent.AddBehaviour("avoid", avoid, 0);
        agent.AddBehaviour("seperation", seperation, 0);
        agent.AddBehaviour("cohesion", cohesionSteer, 1);

        //agent.AddBehaviour("look", new LWYGSteer(), 0);

        agent.MaxVelocity = 4.0f;
        agent.MaxAcceleration = 4.0f;
        agent.MaxAngularVelocity = 2 * Mathf.PI;

        wander.WanderOrientation = Random.Range(-Mathf.PI, Mathf.PI);
    }
	
	public void Update (Agent agent, out IAgentState nextState)
	{
        Initialise(agent);

		nextState = this;
		
		/// camper is attacked -> Evade
		//if (true) { 
		//	nextState = new CamperEvade ();
		
		//}
		
		//float flockingRadius = 5f; // TODO: move this setting to configuration file

		//var agents = agent.GetAgentsInArea(flockingRadius).Where(a => a is Camper);
		
		/*foreach(var a in agents) {
			if(a is Camper) {
				return;
			}
		}*/

		/// camper has no company -> Idle
		//nextState = new CamperIdle ();
	}
	
}


