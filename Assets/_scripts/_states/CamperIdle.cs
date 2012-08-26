using System;
using System.Linq;

using UnityEngine;

public class CamperIdle : AgentState
{
	public void InitAction()
	{
		WanderSteer wanderSteer = new WanderSteer();
		DebugUtil.Assert(agent != null);
		agent.AddBehaviour("wanderSteer", wanderSteer, 1);
	}

	public void ExitAction()
	{
		agent.RemoveBehaviour("wanderSteer");
	}

	public override void Update(out Type nextState)
	{
		nextState = GetType();

		int numAgents = agent.GetAgentsInArea(Config.DefaultCamperFlockRadius).
			Where(a => a is Camper).Count(); 

		Debug.Log("Camper: nearby campers:" + numAgents);
		
		/// camper is close to other campers -> Flock
		if (numAgents > 0) {
			nextState = typeof(CamperFlock);
		}
	}
} 