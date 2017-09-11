using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class KeyboardUtility
{
	public static int detectPressedKey()
	{
		foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(kcode))
				return (int)kcode;
		}
		return 0;
	}
}


