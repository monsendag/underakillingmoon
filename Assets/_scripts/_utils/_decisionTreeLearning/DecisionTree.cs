using System;
using System.Linq;
using System.Collections.Generic;

public class DecisionTree : TreeNode
{
	Dictionary<string, DecisionTree> branches = new Dictionary<string,DecisionTree>();

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

	public void AddBranch(string label, DecisionTree branch)
	{
		branches.Add(label, branch);
	}

	public String GetId()
	{
		return GetHashCode().ToString();
	}

	public Classification Test(Example example)
	{
		// we're done with the recursion.
		//The test is passed if this leaf node's label equals the test's classification.
		if (Classification != null) {
			return Classification;
		}

		foreach (var branch in branches) {
			//if(branch.)
		}
		//If Index isn't -1, choose the correct branch and continue the test.
		return null;
		//return branches[example.Attributes[].Test(example);
	}

	
	public static DecisionTree Create(List<Example> examples)
	{
		return Create(examples, null);
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
	static DecisionTree Create(List<Example> examples, List<Example> parentExamples)
	{

		// no examples. Return a leaf with the plurality value of the parent
		if (examples.Count() == 0) { 
			Console.WriteLine("no examples left");
			return PluralityValue(parentExamples);
		}
		// all examples has the same classification. Return a leaf node with it
		else if (examples.GroupBy(ex => ex.Classification).Distinct().Count() == 1) {
			Console.WriteLine("all examples has the same classification");
			Console.WriteLine(examples.GroupBy(ex => ex.Classification).Distinct());
			return new DecisionTree(examples.First().Classification);
		}
		// no attributes. Return a leaf node with the plurality value
		else if (examples.First().GetAttributes().Count() == 0) { 
			Console.WriteLine("No attributes left to construct from.");
			return PluralityValue(examples);
		} 
		// make a new branch
		else {

			Console.WriteLine("Construct top node.");
			// find most important attribute
			Attribute important = SelectAttribute(examples);

			Console.WriteLine("Selected attribute: " + important);

			// instantiate a branch
			var tree = new DecisionTree(important);  
			// allocate examples we're gonna use for creating the branch
			List<Example> branchExamples;
			// loop through the values for the attribute and select matching examples 

			Console.WriteLine(important.Values);

			foreach (var value in important.Values) {
				// copy all examples which has the value of the attribute
				branchExamples = examples.Where(ex => ex [important] == value).ToList();

				// remove the selected attribute from all examples
				foreach (var ex in branchExamples) {
					ex.Remove(important);
				}
				// Recursively create branch and add it to the tree with the value as edge label
				tree.AddBranch(value.ToString(), Create(branchExamples, examples));
			}
			return tree;
		}
	}
	
	/// <summary>
	/// Selects the attribute.
	/// </summary>
	/// <returns>
	/// The attribute.
	/// </returns>
	/// <param name='examples'>
	/// Examples.
	/// </param>
	static Attribute SelectAttribute(List<Example> examples)
	{
		Attribute important = examples.First().GetAttributes().First();
		double gain, max = Double.MinValue;
		
		foreach (var attribute in examples.First().GetAttributes()) {
			gain = Importance.Random(attribute, examples);
			if (gain > max) {
				important = attribute;
			}
		}
		return important;
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
		string str = "";
		//string str = string.Join(", ", _attributes) + " => " + _classification.ToString();
		if (Classification != null) {
			str += Classification;
		} else {
			str += Attribute;
		}

		foreach (var branch in branches) {
			str += branch;
		}
		return str;
	}

	public string GetLabel()
	{
		return Classification != null ? Classification.ToString() : Attribute.ToString();
	}

	public String ToTGF()
	{
		string str = String.Format("{0} {1}", GetId(), GetLabel());

		str += "\n#";

		foreach (var branch in branches) {
			str += String.Format("\n{0} {1} {2}", GetId(), branch.Value.GetId(), branch.Key);
		}
		return str;
	}
}


