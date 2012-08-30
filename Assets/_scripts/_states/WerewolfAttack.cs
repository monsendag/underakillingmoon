using System;
using UnityEngine;

public class WerewolfAttack : AgentState
{
	Agent target;

    FrictionSteer _friction = new FrictionSteer();
    FaceSteer _face = new FaceSteer();
    ArriveSteer _arrive = new ArriveSteer();

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
        agent.AddBehaviour("arrive", _arrive, 0);
        //agent.AddBehaviour("pursue", _pursue, 0);
        _face.LocalTarget = target.KinematicInfo;
        _arrive.Target = target.KinematicInfo;
        _arrive.SlowRadius = 3.0f;
        _arrive.TargetRadius = 1.0f;

       // _pursue.LocalTarget = target.KinematicInfo;
        //_pursue.MaxAcceleration = 16.0f;
        //_pursue.MaxPrediction = 0.1f;
        
	}

	public void ExitAction()
	{
        agent.RemoveBehaviour("friction");
        agent.RemoveBehaviour("face");
        agent.RemoveBehaviour("arrive");
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
		}
	}
}