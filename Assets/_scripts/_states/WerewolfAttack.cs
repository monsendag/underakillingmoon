using System;

public class WerewolfAttack : AgentState
{
	Agent target = null;

	public void InitAction()
	{

	}

	public void ExitAction()
	{

	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();
		 
		//  Has target, target not in range -> Charge
		if (target != null && agent.distanceTo(target) > Config.DefaultWerewolfChargeRange) {
			nextState = typeof(WerewolfCharge);
		}
	}
}