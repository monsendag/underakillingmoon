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
    private IDecision _decision;
    private DecisionNode _leftNode; // Occurs when decision false.
    private DecisionNode _rightNode; // Occurs when decision true.
    private Type _outputType;

    public IDecision Decision { set { _decision = value; } }
    public DecisionNode FalseNode { set { _leftNode = value; } }
    public DecisionNode TrueNode { set { _rightNode = value; } }
    public Type OutputType { set { _outputType = value; }}

    DecisionNode(IDecision decision, DecisionNode leftNode = null, DecisionNode rightNode = null,
        Type outputType = null)
    {
        _decision = decision;
        _leftNode = leftNode;
        _rightNode = rightNode;
        _outputType = outputType;
    }

    /* Returns a type to transition the state machine to, or NULL if no 
     * suggested output type exists.
     */
    Type GetDecisionOutput()
    {
        if (_outputType != null)
        {
            return _outputType;
        }

        if (_decision.Decide())
        {
            if (_rightNode == null) { return null; }
            return _rightNode.GetDecisionOutput();
        }
        else
        {
            if (_leftNode == null) { return null; }
            return _leftNode.GetDecisionOutput();
        }

    }

}
