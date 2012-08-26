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
	private Type _currentState = null;

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
		set {
			Type type = value.GetType();
				
			if (_currentState != null) {
				PostMessage("ExitAction");
			}

			DebugUtil.Assert(States.ContainsKey(type));

			_currentState = type;
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
			Debug.Log(GetType() + ": adding " + state.GetType());
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
		DebugUtil.Assert(_currentState != null);

		
		AgentState agentState = States [_currentState];


		if (_currentState != null) {
			Type nextState;
			agentState.Update(out nextState); // do the transition 
			CurrentState = nextState;
		}

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
		if (_currentState == null) {
			return;
		}
		
		MethodInfo callback = _currentState.GetType().GetMethod(functionName);
		if (callback != null) {
			callback.Invoke(_currentState, args);
		}

		AgentState agentState = States [_currentState];
		// recurse the message down the state machine tree
		if (agentState is AgentStateMachine) {
			(agentState as AgentStateMachine).PostMessage(functionName, args);
		}
	}
}