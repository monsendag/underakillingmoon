using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public static class FSExtension
{
	public static void AddLine(this FileStream fs, string msg)
	{
		byte[] line = new UTF8Encoding(true).GetBytes(msg + "\n");
		fs.Write(line, 0, line.Length);
	}
}

public class RecordAgent : MonoBehaviour
{
	public int MSBetweenUpdates = 500;
	int _nextUpdate;
	FileStream _csvStream;
	List<Werewolf> _wolves = new List<Werewolf>();

	IProperty[] _decisions = 
    {
        new AgentHealth(),
        new TargetHealth(),
        new NearbyAgents<Werewolf>(),
        new NearbyAgents<Camper>(),
        new DistanceToTarget(),
        new DistanceToPlayer(),
        new RecentGunfire()
    };

	// Use this for initialization
	void Start()
	{
		_csvStream = File.Create("./test.csv");
		// Generate a list of all the current werewolves in the scene.
		_wolves = GameObject.FindGameObjectsWithTag("Werewolf").Select(a => a.gameObject.GetComponent<Werewolf>()).ToList();
		// Output the headers.
		var line = "Classification";
		foreach (var decision in _decisions) {
			line += "," + decision.GetPrettyTypeName();
		}

		_csvStream.AddLine(line);

		_nextUpdate = System.Environment.TickCount;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (_decisions.Length == 0) {
			return;
		}
		if (System.Environment.TickCount > _nextUpdate) {
			foreach (var wolf in _wolves) {

				string line = wolf.StateMachine.GetPrettyTypeName();
				for (int i = 0; i < _decisions.Length; ++i) {
					line += "," + (_decisions [i].Get(wolf)).ToString();
				}
				_csvStream.AddLine(line);
			}
			_nextUpdate = System.Environment.TickCount + MSBetweenUpdates;
		}
	}

	void OnApplicationQuit()
	{
		_csvStream.Dispose();
	}
}
