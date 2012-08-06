using UnityEngine;

public class CamperIdle : IAgentState
{
	public CamperIdle ()
	{
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		
		nextState = this;
		
		/// camper is attacked
		if (true) { 
			nextState = new CamperEvade();
		
		}
		
		/// camper is close to other campers -> flock
		if(true) {
			nextState = new CamperFlock();
		
	}
	
}


