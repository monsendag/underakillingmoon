using UnityEngine;

public class CamperEvade : IAgentState
{
	public CamperEvade()
	{
	}
	
	public void Update(Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// camper is killed -> Dead
		if (true) { 
			nextState = new CamperDead();
		
		}
		
		/// camper is not attacked, has company -> Flock 
		if (true) {
			nextState = new CamperFlock();
		}
		
		/// camper is not attacked, has no company -> Idle 
		if (true) {
			nextState = new CamperFlock();
		}
	}
}


