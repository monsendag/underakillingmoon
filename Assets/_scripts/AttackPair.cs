using System.Collections.Generic;
using System.Linq;

public class AttackPair
{
	private static Dictionary<Agent, Agent> pairs = new Dictionary<Agent, Agent>();

	public static void Add(Agent attacker, Agent target)
	{
		pairs.Add(attacker, target);
	}

	public static Agent GetTargetOrNull(Agent attacker)
	{
		Agent value = null;
		return pairs.TryGetValue(attacker, out value) ? value : null;
	}

	public static Agent GetAttackerOrNull(Agent target)
	{
		return pairs.SingleOrDefault(x => x.Value == target).Key;
	}

	public static void RemoveByAttacker(Agent attacker)
	{
		pairs.Remove(attacker);
	}

	public static void RemoveByTarget(Agent attackee)
	{
		pairs.Remove(GetAttackerOrNull(attackee));
	}

	public static bool IsAttacking(Agent attacker)
	{
		return pairs.ContainsKey(attacker);
	}

	public static bool IsTarget(Agent target)
	{
		return pairs.ContainsValue(target);
	}
}

