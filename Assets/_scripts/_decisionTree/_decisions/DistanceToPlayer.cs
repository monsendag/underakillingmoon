using UnityEngine;
using System.Collections;
using System.Linq;

public class DistanceToPlayer : IValue
{
	DistanceToPlayer(){}
	
	public int Decide(Agent agent)
    {
        
		GameObject player = GameObject.FindGameObjectWithTag("Player") as GameObject;
		float dist = Vector2.Distance(agent.KinematicInfo.Position, player.transform.position.To2D());
        dist = Mathf.Clamp(dist, 0.0f, 100.0f);
        dist /= 20;
        return (int) dist;
    }
}
