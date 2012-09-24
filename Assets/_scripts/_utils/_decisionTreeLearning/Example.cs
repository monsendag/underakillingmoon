using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

public class Example : Dictionary<Attribute, Value>
{
	public Classification Classification;

	public Example(params Pair[] values)
	{
		foreach (var pair in values) {
			Add(pair);
		}
	}

	public void Add(Pair pair)
	{
		Add(pair.Attribute, pair.Value);
	}

	public List<Attribute> GetAttributes()
	{
		return new List<Attribute>(Keys);
	}

	public static List<Example> ParseFileStream(Stream fileStream)
	{
		if (fileStream == null) {
			throw new Exception("Can't parse empty filestream.");
		}

		StreamReader reader = new StreamReader(fileStream);

		// First line defines the attribute headers
		String attributeLine = reader.ReadLine();
		var attributes = new List<Attribute>(); 

		foreach (String attributeName in attributeLine.Split(',')) {
			if (!attributeName.Equals("Classification")) {
				Console.WriteLine(attributeName);
				attributes.Add(new Attribute(attributeName));
			}
		}

		// Each of the following lines defines a  training sample
		var examples = new List<Example>();	
		while (!reader.EndOfStream) {
			String line = reader.ReadLine();
			var values = line.Split(',');
			var example = new Example();
			Attribute[] attributeArr = attributes.ToArray();

			example.Classification = Classification.Parse(values [0]);

			Value value;
			Attribute attribute;
			for (var i=0; i<attributeArr.Length; i++) {
				attribute = attributeArr [i];
				value = attribute.GetValue(values [i]);
				example.Add(attribute, value);
			}
			examples.Add(example);
		}

		return examples;
	}

	public static List<Example> ParseResource(string resource)
	{
		Stream stream = Assembly.GetExecutingAssembly().
			GetManifestResourceStream(resource);
		if (stream == null) {
			throw new Exception("Can't find resource."); 
		}
		return ParseFileStream(stream);
	}

	public override string ToString()
	{
		string str = Classification.ToString();
		foreach (var value in Values) {
			str += ", " + value;
		}
		return str;
	}
}