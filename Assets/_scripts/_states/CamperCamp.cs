using System;
using UnityEngine;
public class CamperCamp : AgentStateMachine
{

	public CamperCamp(Agent agent) : base(agent)
	{
		// construction phase. All substates are also constructed
		AddStates(new CamperIdle(), 
		          new CamperFlock());
	}

	public void InitAction()
	{
		CurrentState = typeof(CamperIdle);
	}

	public void ExitAction()
	{

	}
		
	public override void Update(out Type nextState)
	{
		nextState = GetType();

		/// camper is being attacked -> Evade
        
		if (AttackPair.IsTarget(agent)) { 
			Debug.Log("Camper: I'm being attacked. Evade!");
			nextState = typeof(CamperEvade);
		}

        if (agent.Health <= 0)
        {
            nextState = typeof(CamperDead);
            return;
        }
		
		if(Physics.Raycast(agent.KinematicInfo.Position.To3D(), Vector3.down, Mathf.Infinity) == false)
		{
			Debug.Log ("Not over land, destroying");
			GameObject.Destroy(agent.gameObject);
		}
	}
} 