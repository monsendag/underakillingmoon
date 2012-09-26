using UnityEngine;
using System.Collections;
using System.Linq;

public class DistanceToPlayer : IValue
{
    public static float MaxDistance = 15.0f;
    public static int OutputNumber = 5;
	DistanceToPlayer(){}
	
	public int Decide(Agent agent)
    {
        
		GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) { return OutputNumber - 1; }
		float dist = Vector2.Distance(agent.KinematicInfo.Position, player.transform.position.To2D());
        dist = Mathf.Clamp(dist, 0.0f, MaxDistance);
        dist = (OutputNumber - 1) * dist / MaxDistance;
        
        return (int) Mathf.Round(dist);
    }
}
