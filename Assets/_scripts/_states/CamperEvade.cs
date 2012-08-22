using System;

public class CamperEvade : AgentState
{
	private bool attacked;

	public void InitAction()
	{
		attacked = true;

	}

	public void ExitAction()
	{

	}

	public override void Update(out Type nextState)
	{

		nextState = GetType();
		
		/// camper is killed -> Dead
		if (agent.Health == 0) { 
			nextState = typeof(CamperDead);
		}
		
		/// camper is not attacked, has no company -> Idle 
		if (!attacked) {
			nextState = typeof(CamperCamp);
		}
	}

	public void AttackStopNotify()
	{
		attacked = false;
	}
}


