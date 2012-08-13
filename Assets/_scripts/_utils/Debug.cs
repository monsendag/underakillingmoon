/**
 * Provides some basic utils form debugging the application.
 **/
using UnityEngine;
using System.Collections;
using System;

public class AssertException : Exception
{
	public AssertException() : base("Failed Assert")
	{
	}
}

public class DebugUtil
{
	public static void Assert(bool condition)
	{
		if (!condition)
			throw new AssertException();
	}

}
