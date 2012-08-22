using System;

public class WerewolfPatrol : AgentState
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

		uint distance = 5;
		/// Target in range for charge
		if (distance < Config.DefaultWerewolfChargeRange) {
			nextState = typeof(WerewolfCharge);
		}
	}
}

