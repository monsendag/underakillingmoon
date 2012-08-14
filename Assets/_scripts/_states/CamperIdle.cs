using UnityEngine;

public class CamperIdle : AgentStateMachine
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
		
		/// camper is attacked -> Evade
		if (true) { 
			nextState = new CamperEvade();
		}
		
		/// camper is close to other campers -> Flock
		if (true) {
			nextState = new CamperFlock();
		}
	}
} 