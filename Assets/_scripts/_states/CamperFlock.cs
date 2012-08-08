using UnityEngine;
using System.Collections.Generic;

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
		List<Agent> agents = agent.getAgentsInArea(flockingRadius);
		
		foreach(var a in agents) {
			if(a.GetType() == typeof(Camper)) {
				return;
			}
		}

		/// camper has no company -> Idle
		nextState = new CamperIdle ();
	}
	
}


