using UnityEngine;
using System.Collections;

// Determines whether the wolf recently heard gunfire.
public class RecentGunfire : IValue
{
    public int Decide(Agent agent)
    {
        // Find the player object.
        return 0;

        //int health = Mathf.Clamp((int)agent.Health, 0, 100);
        //health = (int)(agent.Health / 20);
        //return health;
    }

    public string GetPrettyTypeName()
    {
        return "RecentGunfire";
    }
}
