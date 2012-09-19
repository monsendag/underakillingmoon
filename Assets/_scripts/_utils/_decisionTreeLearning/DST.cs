using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public class DST
{
	public DST()
	{
		// parse exampleData
		List<Example> trainingExamples = parseExamples();

		// draw tree: 
		draw(CreateDecisionTree(trainingExamples));
	}

	void draw(TreeNode tree)
	{

	}

	/// <summary>
	/// Parses ...
	/// </summary>
	/// <returns>
	/// A list of examples
	/// </returns>
	List<Example> parseExamples()
	{
		String url = "https://docs.google.com/spreadsheet/fm?id=taiS1sFWQ3oA084dPKjxHOQ.11523487706387223811.3944155610592869218&fmcmd=5&gid=0";
		String localfile = "test.csv";
		
		var client = new WebClient();
		client.DownloadFile(url, localfile);

		return null;
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
	TreeNode CreateDecisionTree(List<Example> examples, List<Example> parentExamples)
	{
		// no examples. Return a leaf with the plurality value of the parent
		if (examples.Count() == 0) { 
			return PluralityValue(parentExamples);
		}
		// all examples has the same classification. Return a leaf node with it
		else if (examples.GroupBy(ex => ex.Classification).Distinct().Count() == 1) { 
			return new TreeNode(examples.First().Classification);
		}
		// no attributes. Return a leaf node with the plurality value
		else if (examples.First().GetAttributes().Count() == 0) { 
			return PluralityValue(examples);
		} 
		// make a new branch
		else {
			// find most important attribute
			Attribute important = SelectAttribute(examples); 
			// instantiate a branch
			var tree = new TreeNode(important.Value);  
			// allocate examples we're gonna use for creating the branch
			List<Example> branchExamples;
			// loop through the values for the attribute and select matching examples 
			foreach (var value in important.Values) {
				// copy all examples which has the value of the attribute
				branchExamples = examples.Where(ex => ex [important] == value).ToList();
				// remove the selected attribute from all examples
				foreach (var ex in branchExamples) {
					ex.Remove(important);
				}
				// Recursively create branch and add it to the tree
				tree.AddBranch(CreateDecisionTree(branchExamples, examples));
			}
			return tree;
		}
	}

	TreeNode CreateDecisionTree(List<Example> examples)
	{
		return CreateDecisionTree(examples, null);
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
	Attribute SelectAttribute(List<Example> examples)
	{
		return null;
	}


	/// <summary>
	/// Returns a random im
	/// </summary>
	/// <returns>
	/// The random.
	/// </returns>
	/// <param name='examples'>
	/// Examples.
	/// </param>
	double ImportanceRandom(Example[] examples)
	{
		return new Random().NextDouble();
	}


	// TODO: figure out how entropy works with variable # of values
	
	
	// the entropy of a random variable V with values Vk, each with probability P(Vk):
	// The entropy of the attribute:
	// 
	double Entropy(double q, int n)
	{
		//the entropy of a n-ary random variable that is true with probability q:
		return -(q * Math.Log(q, n) + (1 - q) * Math.Log(1 - q, n));
	}

	double Remainder(Attribute attribute, List<Example> examples)
	{
		/*
		 * the sum from k = 1 to d 
		 * d = # of distinct values dividing the training set into subsets E1 .. Ed
		 * each subset Ek has examples a various different classifications. pk, nk...
		 * 
		 * 
		SUM( (( #Pk + #Nk + #koko ...) / total # of samples )) * Entropy(pk / #Pk+#Nk+#koko) 

		*/

		double sum = 0;
		double entropy;
		foreach (var value in attribute.Values) {
			//subset
			List<Example> cis = examples.Where(ex => ex [attribute] == value).ToList(); 
			entropy = 0;

			foreach (var classification in cis.Select(ex => ex.Classification)) {
				//TODO FIX
				//entropy += Entropy(cis [attribute].Where());
			}
			sum += cis.Count() / examples.Count() * entropy;
		}
		return sum;
	}

	/*
	we're looking at how much information an attribute gains.
	Entropy is a measure of the uncertainty of a random variable; 
	acquisition of information corresponds to a reduction in entropy
	*/
	double ImportanceInfoGain(Attribute attribute, List<Example> examples)
	{
		int count;
		double totalEntropy = 0;
		foreach (var value in attribute.Values) {
			// get the number of samples where the attribute has the current value
			count = examples.Where(ex => ex [attribute] == value).Count();
			// get the entropy of the values probability
			totalEntropy += Entropy(count / attribute.Values.Count(), attribute.Values.Count());
		}

		//The information gain from the attribute test on A is the expected reduction in entropy
		return (totalEntropy - Remainder(attribute, examples));
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
	public TreeNode PluralityValue(List<Example> examples)
	{
		object value = examples.GroupBy(e => e.Classification).
						OrderByDescending(gp => gp.Count()).First();
		return new TreeNode(value);
	}

}

