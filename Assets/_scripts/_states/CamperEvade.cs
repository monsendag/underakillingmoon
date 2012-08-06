using UnityEngine;

public class CamperEvade : IAgentState
{
	public CamperEvade ()
	{
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// camper is killed
		if (true) { 
			nextState = new CamperDead ();
		
		}
		
		/// camper is not attacked anymore 
		if (true) {
			nextState = new CamperFlock ();
		}
	}
}


