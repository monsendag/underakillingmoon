using UnityEngine;

public class CamperFlock : IAgentState
{
	public CamperFlock ()
	{
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		
		nextState = this;
		
		/// camper is attacked
		if (true) { 
			nextState = new CamperEvade ();
		
		}
		
		/// camper has no flock, becomes idle
		if (true) {
			nextState = new CamperIdle ();
		}
	}
	
}


