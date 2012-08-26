using System;

public class WerewolfEvade : AgentState
{
	Agent attacker;

	public void InitAction()
	{
		ISteeringBehaviour EvadeSteer = new EvadeSteer(agent);
		agent.AddBehaviour("evadeSteer", EvadeSteer, 0);
		
	}

	public void ExitAction()
	{
		agent.RemoveBehaviour("evadeSteer");
	}

	public override void Update(out Type nextState)
	{
		
		nextState = GetType();

		// TODO: implement not being attacked
		if (true) {
			nextState = typeof(WerewolfHunt);
		}

		/// camper is being attacked -> Evade
		if (AttackPair.IsTarget(agent)) { 
			nextState = typeof(WerewolfEvade);
		}
	}
}


