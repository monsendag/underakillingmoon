using System;
using System.Linq;

public class CamperIdle : AgentState
{
	public void InitAction()
	{
		//TODO: add relevant behaviours
	}

	public void ExitAction()
	{
		//TODO: remove relevant behaviours
	}

	public override void Update(out Type nextState)
	{
		nextState = GetType();

		int numAgents = agent.GetAgentsInArea(Config.DefaultCamperFlockRadius).
			Where(a => a is Camper).Count(); 
		
		/// camper is close to other campers -> Flock
		if (numAgents > 0) {
			nextState = typeof(CamperFlock);
		}
	}
} 