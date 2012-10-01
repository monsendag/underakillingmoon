using UnityEngine;
using System.Collections;

public interface IValue
{
    /// <summary>
    /// Given a particular agent, decide which category 
    /// the agent is in.
    /// </summary>
    /// <param name="agent"> The agent to test. </param>
    /// <returns> Returns a positive integer. </returns>
    int Decide(Agent agent);
}