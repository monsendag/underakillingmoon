using UnityEngine;

public class WerewolfIdle : AgentStateMachine
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

		Agent target = null, attacker = null;

		uint distance = 5;
		/// Target in range for charge
		if (distance < Config.WerewolfChargeRange) {
			nextState = new WerewolfCharge();
		}
	}
}

