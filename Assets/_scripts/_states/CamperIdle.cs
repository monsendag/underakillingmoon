using UnityEngine;

public class CamperIdle : IAgentState
{
	public CamperIdle ()
	{
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// camper is attacked -> Evade
		if (true) { 
			nextState = new CamperEvade ();
		}
		
		/// camper is close to other campers -> Flock
		if (true) {
			nextState = new CamperFlock ();
		}
	}
} 