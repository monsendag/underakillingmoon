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

	public void AddStates(params AgentStateMachine[] states)
	{
		foreach (AgentStateMachine state in states) {
			States.Add(state.GetType(), state);
		}
	}

	public void SetStates(params AgentStateMachine[] states)
	{
		Debug.Log("" + states);

		foreach (AgentStateMachine state in states) {
			Debug.Log("" + state);
			Debug.Log("" + state.GetType());
			States.Add(state.GetType(), state);


			Debug.Log("after");
		}
		CurrentState = states.First();
	}

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

	public AgentStateMachine()
	{

	}

	public AgentStateMachine(Agent agent)
	{
		_agent = agent;
	}

	// overridden: this one is called on all active states
	public virtual void Update(out AgentStateMachine state)
	{
		state = null;
	}

	// Top level update. This is called by the agent
	public void Update()
	{
		if (CurrentState != null) {
			AgentStateMachine nextState;
			CurrentState.Update(out nextState);
			CurrentState = nextState;
		}
	}

	public AgentStateMachine CurrentState {
		get { return _currentState; }
		set { SetState(value.GetType()); }
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