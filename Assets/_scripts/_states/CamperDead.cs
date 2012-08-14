using UnityEngine;

public class CamperDead : AgentStateMachine
{
	public void initAction()
	{

	}

	public void exitAction()
	{

	}
	
	public new void Update(out AgentStateMachine nextState)
	{
		nextState = this;
		
		/// timer... wake up as werewolf.. What should happen here?
		if (true) { 
			
		}
	}
}


