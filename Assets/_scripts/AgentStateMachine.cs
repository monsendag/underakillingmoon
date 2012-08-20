using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class AgentStateMachine
{
	private Agent _agent;
	public Dictionary<Type,AgentStateMachine> States = 
		new Dictionary<Type, AgentStateMachine>();
	private AgentStateMachine _currentState;

	protected Agent agent {
		get { return _agent;}
	}

	public AgentStateMachine()
	{

	}

	public AgentStateMachine(Agent agent)
	{
		_agent = agent;
	}

	public AgentStateMachine CurrentState {
		get { return _currentState; }
		set { SetState(value.GetType()); }
	}


	/// <summary>
	/// Adds the given states to the internal list of states.
	/// </summary>
	/// <param name='states'>
	/// States.
	/// </param>
	public void AddStates(params AgentStateMachine[] states)
	{
		foreach (AgentStateMachine state in states) {
			States.Add(state.GetType(), state);
		}
	}

	/// <summary>
	/// Removes all states, then sets them to the given parameters,
	/// then sets CurrentState to the first parameter.
	/// </summary>
	/// <param name='states'>
	/// States.
	/// </param>
	public void SetStates(params AgentStateMachine[] states)
	{
		States.Clear();
		foreach (AgentStateMachine state in states) {
			States.Add(state.GetType(), state);
		}
		CurrentState = states.First();
	}

	/// <summary>
	/// Sets CurrentState to given state.
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	public void SetState(Type type)
	{
		if (_currentState != null) {
			SendStateMessage("ExitAction", null);
		}

		DebugUtil.Assert(States.ContainsKey(type));
		AgentStateMachine state = States [type];

		state._agent = agent;
		_currentState = state;
		SendStateMessage("InitAction", null);
	}


	// overridden: this one is called on all active states
	public virtual void Update(out AgentStateMachine state)
	{
		state = null;
	}
	/// <summary>
	/// Top level update. This is called by the agent
	/// </summary>
	public void Update()
	{
		if (CurrentState != null) {
			AgentStateMachine nextState;
			CurrentState.Update(out nextState);
			CurrentState = nextState;
		}
	}



	public void SendStateMessage(string functionName, object[] args)
	{
		MethodInfo callback = _currentState.GetType().GetMethod(functionName);
		if (callback != null) {
			callback.Invoke(_currentState, args);
		}

		// recurse the message
		if (_currentState.CurrentState != null) {
			_currentState.CurrentState.SendStateMessage(functionName, args);
		}
	}
}