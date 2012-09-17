using UnityEngine;
using System.Collections;
using System.Linq;

public class DistanceToPlayer : IDecision
{
	Agent _agent;
	float  _threshold;
	
	DistanceToPlayer(Agent agent, uint threshold){
		_agent = agent;
		_threshold = threshold;
	}
	
	public bool Decide(){
		GameObject player = GameObject.FindGameObjectWithTag("Player") as GameObject;
		return Vector2.Distance(_agent.KinematicInfo.Position, player.transform.position.To2D()) < _threshold;
	}
}
