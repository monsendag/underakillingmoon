using UnityEngine;
using System.Collections;

// Determines whether the wolf recently heard gunfire.
public class RecentGunfire : IProperty
{
	public static float MaxTime = 4.0f;
	public static int OutputNumber = 5;
	public static float ReachDistance = 8.0f;

	public int Get(Agent agent)
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (player == null) {
			return 0;
		}
		PlayerMovement movement = player.GetComponent<PlayerMovement>();
		if (movement == null) {
			return 0;
		}
		int largestValue = 0;
		foreach (var gunshot in movement.GunShots) {
			if (Vector2.Distance(agent.KinematicInfo.Position, gunshot.Location) > ReachDistance) {
				continue;
			}

			int value = (int)((OutputNumber - 1)
				* (MaxTime - (Time.time - gunshot.TimeStamp)) / MaxTime);
			largestValue = Mathf.Max(value, largestValue);
		}
		return largestValue;
	}

	public string GetPrettyTypeName()
	{
		return "RecentGunfire";
	}
}
