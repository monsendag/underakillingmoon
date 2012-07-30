/**
 * An AgentState is a behavioural state. It dictates the agents current
 * goals and knowledge. It can modify steering states, as well as other states.
 **/
using UnityEngine;
using System.Collections;

public interface IAgentState
{
    /// <summary>
    /// Execute a logical step for the agent.
    /// </summary>
    /// <param name="agent">
    ///     The agent this state belongs to.
    /// </param>
    /// <param name="nextState">"
    ///     The next state to move to. Return null to stay on this state.
    /// </param>
    void Update(Agent agent,  out IAgentState nextState);
}
