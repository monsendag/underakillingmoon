using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The AgentStateMachine is an implementation of an Hierarchial State machine.
/// Because of this, it is also an AgentState.
/// </summary>
public class AgentStateMachine : AgentState
{
	public Dictionary<Type,AgentState> States = new Dictionary<Type, AgentState>();
	Type _currentState = null;

	/// <summary>
	/// Top level constructor. Always called from the parenting Agent.
	/// Stores a reference to the agent for use in transitions.
	/// </summary>
	/// <param name='agent'>
	/// Agent.
	/// </param>
	public AgentStateMachine(Agent agent) : base(agent)
	{
	}

	protected Type CurrentState {
		get { return _currentState; }
		set {
			if (_currentState == value) {
				return;
			}

			if (_currentState != null) {
				PostMessage("ExitAction");
			}

			if (!States.ContainsKey(value)) {
				Debug.Log("States does not exist: " + value);
			}

			DebugUtil.Assert(States.ContainsKey(value));

			_currentState = value;
			PostMessage("InitAction");
		}

	}

	/// <summary>
	/// Adds the states.
	/// </summary>
	/// <param name='states'>
	/// States.

	public void AddStateArr(AgentState[] states)
	{
		foreach (AgentState state in states) {
			state.agent = agent;
			States.Add(state.GetType(), state);
		}
	}

	/// <summary>
	/// Adds the given states to the internal list of states.
	/// Accepts an infinte number of states.
	/// </summary>
	/// <param name='states'>
	/// States.
	/// </param>
	public void AddStates(params AgentState[] states)
	{
		AddStateArr(states);
	}

	/// <summary>
	/// Removes all states in storage, then adds given states
	/// and sets CurrentState to the first parameter.
	/// </summary>
	/// <param name='states'>
	/// States.
	/// </param>
	public void SetStates(params AgentState[] states)
	{
		States.Clear();
		AddStateArr(states);
		CurrentState = states.First().GetType();
	}


	/// <summary>
	/// Calls update on CurrentState for every state machine in the hierarchy.
	/// </summary>
	public void Update()
	{
		DebugUtil.Assert(CurrentState != null);

		AgentState agentState = States [CurrentState];

		Type nextState;
		agentState.Update(out nextState); // do the transition 
		CurrentState = nextState;

		if (agentState is AgentStateMachine) {
			(agentState as AgentStateMachine).Update();
		}
	}

	/// <summary>
	/// Posts a message to the current states in the hierarchy.
	/// </summary>
	/// <param name='functionName'>
	/// The name of the function to be called
	/// </param>
	/// <param name='args'>
	/// (optional) an infinite number of arguments to be called.
	/// </param>
	public void PostMessage(string functionName, params object[] args)
	{

		if (CurrentState == null) {
			return;
		}

		AgentState agentState = States [CurrentState];

		MethodInfo callback = CurrentState.GetMethod(functionName);
		if (callback != null) {
			callback.Invoke(agentState, args);
		}

		// recurse the message down the state machine tree
		if (agentState is AgentStateMachine) {
			(agentState as AgentStateMachine).PostMessage("recursive" + functionName, args);
		}
	}
}