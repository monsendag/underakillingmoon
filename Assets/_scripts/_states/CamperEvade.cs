using System;

public class CamperEvade : AgentState
{
	private Agent attacker;

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
		
		/// camper is killed -> Dead
		if (agent.Health == 0) { 
			nextState = typeof(CamperDead);
		}
		
		/// camper is not attacked, has no company -> Idle 
		if (!AttackPair.IsTarget(agent)) {
			nextState = typeof(CamperCamp);
		}
	}
}


