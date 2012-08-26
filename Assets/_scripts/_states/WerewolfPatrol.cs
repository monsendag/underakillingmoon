using System;
using System.Linq;
using UnityEngine;

public class WerewolfPatrol : AgentState
{
	Agent target;

	public void InitAction()
	{
		target = null;
		AttackPair.RemoveByAttacker(agent);
	}

	public void ExitAction()
	{

	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();

		// search for nearby campers
		target = agent.GetAgentsInArea(Config.DefaultWerewolfVIsionRange) 
			.Where(c => c is Camper) // we only like Camper meat
			.OrderBy(a => agent.distanceTo(a)) // order by distance
			.FirstOrDefault(); // select closest

		// Found a target -> Charge towards it
		if (target != null) {
			AttackPair.Add(agent, target);
			nextState = typeof(WerewolfCharge);
		}
	}
}

