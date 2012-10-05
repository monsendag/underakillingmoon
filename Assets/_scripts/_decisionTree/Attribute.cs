using System;
using System.Linq;
using System.Collections.Generic;


public class Attribute
{
	static Dictionary<string, Attribute> attrs = new Dictionary<string, Attribute>();

	public string Label;
	// list of possible values
	public List<Value> Values = new List<Value>();

	Attribute(String label)
	{
		Label = label;
	}

	public override string ToString()
	{
		return Label;
	}

	public void AddValue(Value value)
	{
		Values.Add(value);
	}

	public Value GetValue(string value)
	{
		foreach (var val in Values) {
			if (val.ToString().Equals(value)) {
				return val;
			}
		}
		Value newVal = Value.Parse(value);
		AddValue(newVal);
		return newVal;
	}

	public static Attribute Get(String name)
	{
		if (!attrs.ContainsKey(name)) {
			attrs [name] = new Attribute(name);
		}

		return attrs [name];
	}

	public double Importance(List<Example> examples)
	{
		//return First(examples);
		//return Infogain(examples);
		return Infogain(examples);
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
	double Random(List<Example> examples)
	{
		return new Random().NextDouble();
	}
	
	
	/*
	we're looking at how much information an attribute gains.
	Entropy is a measure of the uncertainty of a random variable; 
	acquisition of information corresponds to a reduction in entropy
	So we're basically looking for the attribute with the lowest amount of entropy. 
	*/
	double Infogain(List<Example> examples)
	{

		// the number of distinct classifications
		double numClassifications = examples.Select(e => e.Classification).Distinct().Count();
		// the number of each classification
		int count; 
		// the probability of each classification
		List<double> probabilities = new List<double>();

		// for each value, figure out how many distinct classifications it creates
		foreach (var value in Values) {
			// the number distinct classifications given with the current value 
			count = examples.Where(ex => ex [this] == value)
				.Select(e => e.Classification).Distinct().Count();
			// add the probability of the classification given the current value
			probabilities.Add(count / numClassifications);
		}
		//The information gain is the expected reduction in entropy
		return Entropy(probabilities) - Remainder(examples);
	}
	
	double Remainder(List<Example> examples)
	{

		double P;
		double sum = 0;
		int count;
		List<double> cs;
		List<Example> examplesWithValue;
		foreach (var value in Values) {
			//subset
			examplesWithValue = examples.Where(ex => ex [this] == value).ToList(); 

			P = (examplesWithValue.Count() / examples.Count());

			count = examples.Where(ex => ex [this] == value)
				.Select(e => e.Classification).Distinct().Count();

			cs = examples.Where(ex => ex [this] == value)
				.GroupBy(e => e.Classification)
					.Select(x => ((double)x.Count()) / count).ToList();

			sum += P * Entropy(cs);
		}
		return sum;
	}

	double Entropy(List<double> probabilities)
	{
		double entropy = 0;
		foreach (double p in probabilities) {
			entropy += p == 0 ? 0 : p * Math.Log(p, 2);
		}
		return -entropy;
	}

}

