using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// A node in a decision tree. The node is either a terminal node, in which
/// case it only needs to contain an OutputType, or contains a decision and
/// left and right nodes.
/// </summary>
class DecisionNode
{
    private IValue _decision;
    private Type _outputType;
    private Dictionary<int, DecisionNode> _children = 
        new Dictionary<int, DecisionNode>();

    public IValue Decision { set { _decision = value; } }
    public Type OutputType { set { _outputType = value; }}


    DecisionNode(IValue decision,
        Type outputType = null)
    {
        _decision = decision;
        _outputType = outputType;
    }

    /// <summary>
    /// Decides which state an agent should be in.
    /// </summary>
    /// <returns> The type of the state the agent should transition to. 
    /// </returns>
    Type GetDecisionOutput(Agent agent)
    {
        if (_outputType != null)
        {
            return _outputType;
        }

        int decision = _decision.Decide(agent);
        if (_children.Keys.Contains(decision))
        {
            return _children[decision].GetDecisionOutput(agent);
        }

        // If this code is reached, it means the decision tree isn't full.
        DebugUtil.Assert(false); 
        return null;

    }

    void AddChild(int label, DecisionNode decision)
    {
        DebugUtil.Assert(decision != null);
        _children.Add(label, decision);
    }
}
