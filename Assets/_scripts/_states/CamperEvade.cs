using System;
using UnityEngine;

public class CamperEvade : AgentState
{
	private Agent attacker;
    private EvadeSteer _evadeSteer = new EvadeSteer();
    private LWYGSteer _look = new LWYGSteer();

	public void InitAction()
	{
        attacker = AttackPair.GetAttackerOrNull(agent);
        if (attacker == null)
        {
            return;
        }
        _evadeSteer.LocalTarget = attacker.KinematicInfo;
		agent.AddBehaviour("evadeSteer", _evadeSteer, 0);
        agent.AddBehaviour("look",_look, 0);
	}

	public void ExitAction()
	{
		agent.RemoveBehaviour("evadeSteer");
        agent.RemoveBehaviour("look");
	}

	public override void Update(out Type nextState)
	{
		nextState = GetType();

		// camper is killed -> Dead

		
		/// camper is not attacked, has no company -> Idle 
		if (!AttackPair.IsTarget(agent)) {
			nextState = typeof(CamperCamp);
            return;
		}
        
        if (agent.Health == 0)
        {
            nextState = typeof(CamperDead);
            return;
        }
        if (attacker == null)
        {
            nextState = typeof(CamperCamp);
        }
       // Debug.DrawLine(agent.transform.position, attacker.transform.position, Color.blue);
	}
}


