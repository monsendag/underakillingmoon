using System;
using UnityEngine;

public class WerewolfAttack : AgentState
{
	Agent target;

    FrictionSteer _friction = new FrictionSteer();
    FaceSteer _face = new FaceSteer();
    PursueSteer _pursue = new PursueSteer();

	public void InitAction()
	{
        target = AttackPair.GetTargetOrNull(agent);

        // Set the werewolf to follow it's target.
        //_seek.Target = target.KinematicInfo;
        //_seek.MaxAcceleration = 16.0f;

       // agent.AddBehaviour("seek", _seek, 0);
       // agent.AddBehaviour("look", _look, 0);
        _friction.VelocityFrictionPercentage = 0.3f;
        agent.AddBehaviour("friction", _friction, 0);
        agent.AddBehaviour("face", _face, 0);
        //agent.AddBehaviour("pursue", _pursue, 0);
        _face.LocalTarget = target.KinematicInfo;
       // _pursue.LocalTarget = target.KinematicInfo;
        //_pursue.MaxAcceleration = 16.0f;
        //_pursue.MaxPrediction = 0.1f;
        
	}

	public void ExitAction()
	{
        agent.RemoveBehaviour("friction");
        agent.RemoveBehaviour("face");
        //agent.RemoveBehaviour("pursue");
        //agent.RemoveBehaviour("seek");
		//AttackPair.RemoveByAttacker(agent);
	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();
		 
		target = AttackPair.GetTargetOrNull(agent);

		// has no target -> Patrol
		if (target == null) {
			nextState = typeof(WerewolfPatrol);
		}

		//  Has target, but not in range for attack -> Charge
		else if (agent.distanceTo(target) > Config.DefaultWerewolfAttackRange) {
            nextState = typeof(WerewolfCharge);
		}

		// chew on it
		// TODO: Throttle this action
		else {
			target.Health -= 1;
			Debug.Log("Werewolf is chewing on its target.");
		}
	}
}