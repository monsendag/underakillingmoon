using UnityEngine;

public class CamperDead : IAgentState
{
	public CamperDead ()
	{
		
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// timer... wake up as werewolf.. What should happen here?
		if (true) { 
			
		}
	}
}


