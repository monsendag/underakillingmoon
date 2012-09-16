using System;
using System.Collections.Generic;
using System.Linq;

public class DST
{

	//private IList<Attribute> attributes = new IList<Attribute>();

	public DST()
	{
		//attributes.Add(new Attribute());
		List<Attribute> attributes = null;

		// parse exampleData
		List<Example> trainingExamples = parseExamples();

		// draw tree: 
		draw(CreateDecisionTree(trainingExamples, attributes));

	}

	public void draw(Tree tree)
	{

	}

	public Example[] parseExamples()
	{
		return null;
	}

	public Tree CreateDecisionTree(List<Attribute> attributes, List<Example> examples)
	{
		return DecisionTreeLearning(attributes, examples, null);
	}

	public Tree DecisionTreeLearning(List<Attribute> attributes, List<Example> examples, List<Example> parentExamples)
	{

		List<object> classifications = new List<object>(); 
		// store all classifications in an array
		foreach (var example in examples) {
			classifications.Add(example.Classification);
		}

		// no examples
		if (examples.Count() == 0) { 
			return pluralityValue(parentExamples);
		}

		// all examples has the same classification
		// return a node with that classification
		else if (classifications.Distinct().Count() == 1) { 
			return new Tree(classifications [0]);
		}

		// no attributes
		else if (attributes.Count() == 0) { 
			return pluralityValue(examples);
		} 

		// make a branch
		else {
			// clone the list of attributes
			List<Attribute> branchAttributes = new List<Attribute>(attributes);

			// find most important attribute
			Attribute important = Argmax(ImportanceInfoGain, attributes, examples); 

			// remove the extracted attribute from branch attribute list
			branchAttributes.Remove(important);

			// allocate a list of relevant examples
			List<Example> branchExamples;

			// instantiate a Tree object
			var tree = new Tree(important.Label);  

			foreach (var value in important.Values) {

				// instantiate branch examples
				branchExamples = new List<Example>();

				// copy all examples which has the value of the attribute
				foreach (Example example in examples) {
					if (example.Values [important] == value) {
						branchExamples.Add(example);
					}
				}

				// add the new branch to the tree. Continue recursively
				tree.AddBranch(DecisionTreeLearning(branchAttributes, branchExamples, examples));
			}
			return tree;
		}
		return null;
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
	public Tree PluralityValue(List<Example> examples)
	{
		object value = examples.GroupBy(e => e.Classification).
						OrderByDescending(gp => gp.Count()).First();

		return new Tree(value);
	}


	double ImportanceRandom(Attribute[] attributes, Example[] examples)
	{
		//	return new Random();
		return 0;
	}


	double ImportanceInfoGain(Attribute[] attributes, Example[] examples)
	{

		int numValues = attributes[0].Values.Count();

		// TODO: figure out how entropy works with variable # of values
		double Entropy = q => 
			-(q * log2(q) + (1 - q) * log2(1 - q)); 

		var numClassifications = examples, match => 
			var count = 0;

			foreach(example in examples) {
				if()
			examples.forEach(function (ex) {
				if (ex.classification == match) count++;
			});
			return count;
		}

		var allOnes = numClassifications(examples, 1);
		var allTwos = numClassifications(examples, 2);

		function Remainder(A) {
			var sum = 0;
			A.values.forEach(function(v) {
				var exs = [];
				examples.forEach(function (ex) {
					if (ex.values[A.id] == v) exs.push(ex);
				});

				if (exs.length == 0) return;

				var pk = numClassifications(exs, 1);
				var nk = numClassifications(exs, 2);

				sum += ((pk + nk) / (allOnes + allTwos)) * B(pk / (pk + nk));
			});
			return sum;
		}
		return (Entropy(allOnes / (allOnes + allTwos)) - Remainder(A));
	
	}

	object Argmax(Func<object, int> function, object[] parameters, params object[] extra)
	{
		object argument; // the argument causing the largest return value
		int max = int.MinValue; // initi
		int result;
		foreach (var parameter in parameters) {
			result = function(parameter);

			if (result > max) {
				max = result;
				argument = parameter;
			}
		}
		
		return argument;
	}


}

