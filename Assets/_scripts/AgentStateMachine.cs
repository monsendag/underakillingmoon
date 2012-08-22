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
	private AgentState _currentState = null;

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

	public AgentState CurrentState {
		get { return _currentState; }
		set { SetState(value.GetType()); }
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
		foreach (AgentState state in states) {
			States.Add(state.GetType(), state);
		}
	}

	/// <summary>
	/// Removes all states in storage, then adds given states
	/// and sets CurrentState to the first parameter.
	/// </summary>
	/// <param name='states'>
	/// States.
	/// </param>
	public void SetStates(AgentState newState, params AgentState[] states)
	{
		States.Clear();
		if (states != null) {
			foreach (AgentState state in states) {
				States.Add(state.GetType(), state);
			}	
		}
		CurrentState = newState;
	}

	/// <summary>
	/// Sets CurrentState to given state.
	/// Calls ExitAction on previous state 
	/// and InitAction on the new state
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	public void SetState(Type type)
	{
		if (_currentState != null) {
			PostMessage("ExitAction");
		}

		DebugUtil.Assert(States.ContainsKey(type));
		AgentState state = States [type];

		_currentState = state;
		PostMessage("InitAction");
	}



	/// <summary>
	/// Calls update on CurrentState for every state machine in the hierarchy.
	/// </summary>
	public void Update()
	{
		if (CurrentState != null) {
			Type nextState;
			CurrentState.Update(out nextState); // do the transition 
			SetState(nextState);
		}
		if (CurrentState is AgentStateMachine) {
			(CurrentState as AgentStateMachine).Update();
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

		// recurse the message
		if (_currentState is AgentStateMachine) {
			(_currentState as AgentStateMachine).PostMessage(functionName, args);
		}
	}
}