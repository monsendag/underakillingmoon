using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

public class DecisionTree
{
	Dictionary<Value, DecisionTree> branches = new Dictionary<Value,DecisionTree>();

	// If the tree is a vertice, the attribute defines its label
	public Attribute Attribute = null;

	// The tree is a leaf node if it has a classification
	// determined by the infogain algorithm
	public Classification Classification = null;

	public DecisionTree(Classification classification)
	{	
		Classification = classification;
	}

	public DecisionTree(Attribute attribute)
	{
		Attribute = attribute;
	}

	public void AddBranch(Value label, DecisionTree branch)
	{
		branches.Add(label, branch);
	}

	public String GetId()
	{
		return GetHashCode().ToString().Trim();
	}

	public Classification Test(Example example)
	{
		// if we have a classification, return it
		if (Classification != null) {
			return Classification;
		}

		// recurse
		return branches.Where(b => b.Key == example [b.Value.Attribute]).First().Value.Test(example);
	}

	
	public static DecisionTree Create(List<Attribute> attributes, List<Example> examples)
	{
		return Create(attributes, examples, null);
	}

	/// <summary>
	/// Decision Tree Learning.
	/// Implementation follows the pseudo code on page 702 in Russel/Norvigs AI A Modern Approach 3e. 
	/// 
	/// </summary>
	/// <returns>
	/// A decision tree model to use for making a decision given some example data and their attributes.
	/// </returns>
	/// <param name='attributes'>
	/// Attributes.
	/// </param>
	/// <param name='examples'>
	/// Examples.
	/// </param>
	/// <param name='parentExamples'>
	/// Parent examples.
	/// </param>
	static DecisionTree Create(List<Attribute> attributes, List<Example> examples, List<Example> parentExamples)
	{
		// no examples. Return a leaf with the plurality value of the parent
		if (examples.Count() == 0) { 
			return PluralityValue(parentExamples);
		}
		// all examples has the same classification. Return a leaf node with it
		else if (examples.GroupBy(ex => ex.Classification).Distinct().Count() == 1) {
			return new DecisionTree(examples.First().Classification);
		}
		// no attributes. Return a leaf node with the plurality value
		else if (examples.First().GetAttributes().Count() == 0) {
			return PluralityValue(examples);
		} 
		// make a new branch
		else {
			// find most important attribute
			var important = attributes.OrderByDescending(a => a.Importance(examples)).First();
			// remove the selected attribute from all branch examples
			attributes.Remove(important);
			// instantiate a branch
			var tree = new DecisionTree(important);  
			// allocate examples we're gonna use for creating the branch
			List<Example> branchExamples;
			// loop through the values for the attribute and select matching examples 
			foreach (var value in important.Values) {
				// copy all examples which has the value of the attribute
				branchExamples = examples.Where(ex => ex [important] == value).ToList();
				// Recursively create branch and add it to the tree with the value as edge label
				tree.AddBranch(value, Create(attributes, branchExamples, examples));
			}
			return tree;
		}
	}
	
	/// <summary>
	/// Selects the most common classification value among a set of examples
	/// </summary>
	/// <returns>
	/// The most common value as a Tree node
	/// </returns>
	/// <param name='examples'>
	/// Examples.
	/// </param>
	static DecisionTree PluralityValue(List<Example> examples)
	{
		Classification classification = examples.GroupBy(e => e.Classification).
			OrderByDescending(gp => gp.Count()).First().Key;
		return new DecisionTree(classification);
	}


	public override string ToString()
	{
		return GetLabel();
	}

	/// <returns>
	/// The treenodes label.
	/// </returns>
	public string GetLabel()
	{
		return Classification != null ? Classification.ToString() : Attribute.ToString();
	}

	/// <summary>
	/// Returns a string representation of all nodes in the tree.
	/// </summary>
	public String GetTGFNodes()
	{
		return GetId() + " " + GetLabel() + "\n" + String.Join("", branches.Values.Select(b => b.GetTGFNodes() + "").ToArray());
	}

	/// <summary>
	/// Returns a string representation of all edges in the tree,
	/// Separated by a newline character.
	/// </summary>
	public String GetTGFEdges()
	{
		var branchEdges = String.Join("", branches.Select(p => GetId() + " " + p.Value.GetId() + " " + p.Key + "\n").ToArray());
		var subEdges = String.Join("", branches.Values.Select(b => b.GetTGFEdges()).ToArray());
		return branchEdges + subEdges;
	}

	/// <summary>
	/// Creates a trivial graph representation of the tree.
	/// </summary>
	/// <returns>
	/// A tgf representation of the tree.
	/// </returns>
	public String ToTGF()
	{
		return "\n" + String.Concat(GetTGFNodes(), "#\n", GetTGFEdges()).TrimEnd();
	}

	/// <summary>
	/// Exports a tgf (Trivial Graph Format) file on the desktop.
	/// Useful for seeing how the resulting tree looks. 
	/// The file can be viewed in an editor like yEd.
	/// </summary>
	public void SaveTGFonDesktop()
	{
		var folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		var path = System.IO.Path.Combine(folder, "tree.tgf");
		System.IO.File.WriteAllText(path, ToTGF(), Encoding.Default);
	}
}


