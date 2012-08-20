using System;

public class CamperDead : AgentState
{
	public void InitAction()
	{

	}

	public void ExitAction()
	{

	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();
		/// timer... wake up as werewolf.. What should happen here?
		if (true) { 
			
		}
	}
}


