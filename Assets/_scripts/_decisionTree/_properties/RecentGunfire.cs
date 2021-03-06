using UnityEngine;
using System.Collections;

// Determines whether the wolf recently heard gunfire.
public class RecentGunfire : IProperty
{
	public static float MaxTime = 4.0f;
	public static int OutputNumber = 2;
	public static float ReachDistance = 8.0f;

	public int Get(Agent agent)
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (player == null) {
			return 0;
		}
		PlayerBehaviour movement = player.GetComponent<PlayerBehaviour>();
		if (movement == null) {
			return 0;
		}

		foreach (var gunshot in movement.GunShots) {
			if (Vector2.Distance(agent.KinematicInfo.Position, gunshot.Location) > ReachDistance) {
				continue;
			}
            if (Time.time - gunshot.TimeStamp < MaxTime)
            {
                return 1;
            }
		}
		return 0;
	}

	public string GetPrettyTypeName()
	{
		return "RecentGunfire";
	}
}
