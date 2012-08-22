using System;

public class WerewolfEvade : AgentState
{
	Agent attacker, target;

	public void InitAction()
	{

	}

	public void ExitAction()
	{

	}

	public override void Update(out Type nextState)
	{
		
		nextState = GetType();

		// TODO: implement not being attacked
		if (true) {
			nextState = typeof(WerewolfHunt);
		}

	}

}


