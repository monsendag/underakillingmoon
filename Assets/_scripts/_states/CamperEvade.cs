using System;
using UnityEngine;

public class CamperEvade : AgentState
{
	private Agent attacker;
    private FleeSteer _fleeSteer = new FleeSteer();
    private LWYGSteer _look = new LWYGSteer();
    private CollisionAvoidanceSteer _avoid = new CollisionAvoidanceSteer();
    private ObstacleAvoidSteer _obstacleAvoid = new ObstacleAvoidSteer();

	public void InitAction()
	{
        attacker = AttackPair.GetAttackerOrNull(agent);
        if (attacker == null)
        {
            return;
        }
        _fleeSteer.MaxAcceleration = 5.0f;
        _fleeSteer.Target = attacker.KinematicInfo;
        //_avoid.LookAhead = 1.5f;
        agent.ClearBehaviours();
		agent.AddBehaviour("fleeSteer", _fleeSteer, 0);
        agent.AddBehaviour("look", _look, 0);
        //agent.AddBehaviour("avoid", _avoid, 0);
        agent.AddBehaviour("obstacleAvoid", _obstacleAvoid, 0);
	}

	public void ExitAction()
	{
		agent.RemoveBehaviour("fleeSteer");
        agent.RemoveBehaviour("look");
        agent.RemoveBehaviour("avoid");
        agent.RemoveBehaviour("obstacleAvoid");
	}

	public override void Update(out Type nextState)
	{
		nextState = GetType();

		/// camper is not attacked, has no company -> Idle 
		if (!AttackPair.IsTarget(agent)) {
			nextState = typeof(CamperCamp);
            return;
		}


        // camper is killed -> Dead
        attacker = AttackPair.GetAttackerOrNull(agent);
        _fleeSteer.Target = attacker.KinematicInfo;
        
        if (agent.Health <= 0)
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


