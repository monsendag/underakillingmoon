using System;
using System.Linq;
using System.Collections.Generic;

public static class Importance
{

	/// <summary>
	/// Returns a random im
	/// </summary>
	/// <returns>
	/// The random.
	/// </returns>
	/// <param name='examples'>
	/// Examples.
	/// </param>
	public static double Random(Attribute attribute, List<Example> examples)
	{
		return new Random().NextDouble();
	}


	/*
	we're looking at how much information an attribute gains.
	Entropy is a measure of the uncertainty of a random variable; 
	acquisition of information corresponds to a reduction in entropy
	*/
	public static double Infogain(Attribute attribute, List<Example> examples)
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

	// the entropy of a random variable V with values Vk, each with probability P(Vk):
	// The entropy of the attribute:
	// 
	static double Entropy(double q, int n)
	{
		//the entropy of a n-ary random variable that is true with probability q:
		return -(q * Math.Log(q, n) + (1 - q) * Math.Log(1 - q, n));
	}


	static double Remainder(Attribute attribute, List<Example> examples)
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

}
