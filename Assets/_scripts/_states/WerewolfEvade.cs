using UnityEngine;

public class WerewolfEvade : AgentStateMachine
{
	Agent attacker, target;

	public void initAction()
	{

	}

	public void exitAction()
	{

	}

	public new void Update(out AgentStateMachine nextState)
	{
		nextState = this;

		// TODO: implement not being attacked
		if (true) {
			nextState = new WerewolfHunt();
		}

	}

}


