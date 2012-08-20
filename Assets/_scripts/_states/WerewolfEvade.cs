using UnityEngine;

public class WerewolfEvade : AgentStateMachine
{
	Agent attacker, target;

	public void InitAction()
	{

	}

	public void ExitAction()
	{

	}

	public override void Update(out AgentStateMachine nextState)
	{
		nextState = this;

		// TODO: implement not being attacked
		if (true) {
			nextState = new WerewolfHunt();
		}

	}

}


