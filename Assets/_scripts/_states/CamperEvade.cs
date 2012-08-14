using UnityEngine;

public class CamperEvade : AgentStateMachine
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
		
		/// camper is killed -> Dead
		if (agent.Health == 0) { 
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


