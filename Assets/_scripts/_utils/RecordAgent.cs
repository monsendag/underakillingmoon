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

public class RecordAgent : MonoBehaviour {
    public int MSBetweenUpdates = 10;
    int _nextUpdate;
    FileStream _csvStream;
	List<GameObject> _wolves;

    IDecision[] _decisions = {
        new AgentHealth()//, etc...
    };

	// Use this for initialization
	void Start () {
        _csvStream = File.Create("./test.csv");
		_wolves = GameObject.FindGameObjectsWithTag("Werewolf");
        _nextUpdate = System.Environment.TickCount;
	}
	
	// Update is called once per frame
	void Update () {
        if (System.Environment.TickCount > _nextUpdate)
        {
            for(int j = 0; j < _wolves.Count; ++j){
				string line = "";
	            line += _decisions[0];
	            for (int i = 1; i < _decisions.Length; ++i)
	            {
	                line += "," + _decisions[i].Decide().ToString();
	            }
	            _csvStream.AddLine(line);
			}
            _nextUpdate = System.Environment.TickCount + MSBetweenUpdates;
        }
	}

    void OnApplicationQuit(){
        _csvStream.Dispose();
    }
}
