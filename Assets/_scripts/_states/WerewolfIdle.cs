using UnityEngine;

public class WerewolfIdle : AgentStateMachine
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

		Agent target = null, attacker = null;

		uint distance = 5;
		/// Target in range for charge
		if (distance < Config.DefaultWerewolfChargeRange) {
			nextState = new WerewolfCharge();
		}
	}
}

