using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Werewolf hunt.
/// 
/// This state is a hierarchial state machine, and 
/// should contain WerewolfIdle, WerewolfCharge and WerewolfAttack
/// </summary>

public class WerewolfHunt : AgentStateMachine
{
	static DecisionTree _decisionTree = null;
    private float _reevaluateTime = 0.0f;
    static public float ReevaluationTime = 0.5f;

	public static DecisionTree DecisionTree {
		get {
			if (_decisionTree == null) 
            {
                FileStream stream = new FileStream("./input.csv", FileMode.Open);
                var examples = Example.ParseFileStream(stream);
                stream.Close();
                stream.Dispose();

                var attributes = examples.First().GetAttributes();

                _decisionTree = DecisionTree.Create(attributes, examples);
                _decisionTree.SaveTGFonDesktop();
   
			}
			DebugUtil.Assert(_decisionTree != null);
			return _decisionTree;
		}
	}

	bool beingAttacked = false;

	private static IProperty[] _values = 
    {
        new AgentHealth(),
        new TargetHealth(),
        new NearbyAgents<Werewolf>(),
        new DistanceToTarget(),
        new DistanceToPlayer(),
        new RecentGunfire()
    };

	public WerewolfHunt(Agent agent) : base(agent)
	{
		// add all substates
		AddStates(new WerewolfPatrol(), 
		          new WerewolfCharge(), 
		          new WerewolfAttack(),
                  new WerewolfDead(),
                  new WerewolfEvade()
		);
	}

	public void InitAction()
	{
		CurrentState = typeof(WerewolfPatrol);
		beingAttacked = false;

	}

	public void ExitAction()
	{
		agent.RemoveBehaviour("avoid");
		agent.RemoveBehaviour("obstacleAvoid");
		AttackPair.RemoveByAttacker(agent);
	}

    public override void Update(out Type nextState)
    {
        nextState = GetType();

        if (agent.Health <= 0)
        {
            CurrentState = typeof(WerewolfDead);
        }

        // Create an example, which we can send to the 
        Example example = new Example();
        foreach (var value in _values)
        {
            Attribute atr = Attribute.Get(value.GetPrettyTypeName());
            int val = value.Get(agent);
            example.Add(atr, new Value(val));
        }

        // Ask the decision tree which state we should be in.
        if (Config.UseDecisionTree)
        {
            _reevaluateTime -= Time.deltaTime;
            if (_reevaluateTime < 0)
            {
                _reevaluateTime = ReevaluationTime;
                StateClassification classification = DecisionTree.Test(example) as StateClassification;
                CurrentState = classification.State; // Set the current substate.
            }
            //if (CurrentState == typeof(WerewolfEvade))
            //{
            //    Debug.Break();
            //}
        }
        else if (beingAttacked)
        {
           // We are a cowardly werewolf who runs when shot at.
           nextState = typeof(WerewolfEvade);
           return;
         }

        var target = AttackPair.GetTargetOrNull(agent);
        if (target != null && target.distanceTo(agent) > 10.0)
        {
            AttackPair.RemoveByAttacker(agent);
        }

         var targets = agent.GetAgentsInArea(Config.DefaultWerewolfVisionRange);
         float minDistance = -1.0f;
         target = null;
         foreach (var camper in targets)
         {
            if (camper.GetComponent<Camper>() != null)
            {
               // Perform a raycast check.

               if (minDistance < -1.0f || minDistance < agent.distanceTo(camper) &&
                  !AttackPair.IsTarget(camper))
               {
                  Vector3 direction = (camper.transform.position - agent.transform.position).normalized;
                  var hits = Physics.RaycastAll(agent.transform.position, direction, Config.DefaultWerewolfVisionRange);
                  bool visible = true;
                  foreach (var hit in hits)
                  {
                     if (hit.collider.gameObject != agent.gameObject && hit.collider.gameObject != camper.gameObject)
                     {
                        visible = false;
                     }
                  }
                  if (visible)
                  {
                     target = camper;
                  }
               }
            }
         }

         if (target != null)
         {
            var currentTarget = AttackPair.GetTargetOrNull(agent);
            // Examine the two distance, if the new camper is significantly
            // close then the old one, start chasing the new one.
            if (currentTarget == null ||
              agent.distanceTo(currentTarget) * 1.5 > agent.distanceTo(target) ||
              currentTarget.Health < 0)
            {
              AttackPair.RemoveByAttacker(agent);
              AttackPair.Add(agent, target);
            }
         }
    }

	/// <summary>
	/// The werewolf has take a shot.
	/// </summary>
	public void TakeHit()
	{
		// Swap to evasive state.
		beingAttacked = true;
	}
}