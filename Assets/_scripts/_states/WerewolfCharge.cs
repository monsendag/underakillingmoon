using System;
using UnityEngine;

public class WerewolfCharge : AgentState
{
	Agent target;

    private PursueSteer _pursue = new PursueSteer();
    private LWYGSteer _look = new LWYGSteer();
	public void InitAction()
	{
        target = AttackPair.GetTargetOrNull(agent);
  
        // Set the werewolf to follow it's target.
        _pursue.Target = target.KinematicInfo;
        _pursue.MaxAcceleration = 16.0f;
        _pursue.MaxPrediction = 0.5f;
        agent.AddBehaviour("pursue", _pursue, 0);
        agent.AddBehaviour("look", _look, 0);

	}

	public void ExitAction()
	{
        agent.RemoveBehaviour("pursue");
        agent.RemoveBehaviour("look");
	}

	public override void Update(out Type nextState)
	{
		nextState = GetType();

		target = AttackPair.GetTargetOrNull(agent);
  
		/// Has no target -> go back to Patrolling
		if (target == null) {
			nextState = typeof(WerewolfPatrol);
            return;
		}

		/// Has target, target in range for Attacking -> Attack
		if (target != null && agent.distanceTo(target) < Config.DefaultWerewolfAttackRange) {
			nextState = typeof(WerewolfAttack);
            return;
		}

        Debug.DrawLine(target.transform.position, agent.transform.position + new Vector3(0.0f,0.0f, 1.0f), Color.red);

        _pursue.LocalTarget = target.KinematicInfo;
	}
}

