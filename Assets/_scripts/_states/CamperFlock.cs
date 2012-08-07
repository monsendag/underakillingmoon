using UnityEngine;

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
		
		/// camper has no company -> Idle
		if (true) {
			nextState = new CamperIdle ();
		}
	}
	
}


