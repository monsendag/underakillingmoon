using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CamperFlock : IAgentState
{
	public CamperFlock ()
	{
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		
		nextState = this;
		
		/// camper is attacked -> Evade
		if (true) { 
			nextState = new CamperEvade ();
		
		}
		
		float flockingRadius = 5f; // TODO: move this setting to configuration file
//		List<Agent> agents = agent.getAgentsInArea(flockingRadius).Where(a => a is Camper);
		
		/*foreach(var a in agents) {
			if(a is Camper) {
				return;
			}
		}*/

		/// camper has no company -> Idle
		nextState = new CamperIdle ();
	}
	
}


