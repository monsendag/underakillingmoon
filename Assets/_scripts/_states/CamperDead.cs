using UnityEngine;

public class CamperDead : AgentStateMachine
{
	public void InitAction()
	{

	}

	public void ExitAction()
	{

	}
	
	public override void Update(out AgentStateMachine nextState)
	{
		nextState = this;
		
		/// timer... wake up as werewolf.. What should happen here?
		if (true) { 
			
		}
	}
}


