using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WerewolfPatrol : AgentState
{
	Agent target;
	WaypointSteer _waypointSteer = new WaypointSteer();
    LWYGSteer _look = new LWYGSteer();
    private CollisionAvoidanceSteer _avoid = new CollisionAvoidanceSteer();
    private ObstacleAvoidSteer _obstacleAvoid = new ObstacleAvoidSteer();
	static List<Vector2> _campfires = null;
	static IndexShuffleBag _shuffleBag = null;

	public void InitAction()
	{
		target = null;
		AttackPair.RemoveByAttacker(agent);
		
		if(_shuffleBag == null && _campfires == null){
			_campfires = GameObject.FindGameObjectsWithTag("Campfire")
				.Select(w => MotionUtils.To2D(w.transform.position))
				.OrderBy(w => Vector2.Distance(agent.KinematicInfo.Position, w))
				.ToList();
			
			_shuffleBag = new IndexShuffleBag();
			_shuffleBag.GenerateShuffleBag(_campfires.Count);
		}
		
		_waypointSteer.IndexBag = _shuffleBag;
        _waypointSteer.Waypoints = _campfires;
		_waypointSteer.MaxAcceleration = 16.0f;
        agent.AddBehaviour("waypoint", _waypointSteer, 1);

        //agent.AddBehaviour("avoid", _avoid, 0);
        agent.AddBehaviour("obstacleAvoid", _obstacleAvoid, 0);
        agent.AddBehaviour("look", _look, 0);
	}

	public void ExitAction()
	{
		agent.RemoveBehaviour("waypoint");
        agent.RemoveBehaviour("look");
        agent.RemoveBehaviour("avoid");
        agent.RemoveBehaviour("obstacleAvoid");
    }
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();

		// search for nearby campers

        if (!Config.UseDecisionTree)
        {
            var target = AttackPair.GetTargetOrNull(agent);
            // Found a target -> Charge towards it
            if (target != null)
            {
                nextState = typeof(WerewolfCharge);
            }
        }
	}
}

