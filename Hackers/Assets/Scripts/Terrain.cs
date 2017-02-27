using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Terrain : MonoBehaviour
{
	public const double deadlyThreshold = 0.75;

	Dictionary<string, int>[] concentration = new Dictionary<string, int>[2];

	private double fertilityRatio;
	public double FertilityRatio
	{
		get { return fertilityRatio; }
		set
		{
			fertilityRatio = value;
		}
	}

	private string terrainType;
	public string TerrainType{
		get { return terrainType; }
		set {
			terrainType = value;
		}
	}

	public void setConcentration()
	{
		this.concentration[0] = new Dictionary<string, int>();
		this.concentration[1] = new Dictionary<string, int>();
		this.concentration[0].Add("first str", 0);

	}

	public Terrain (
		string terrainType,
		double fertilityRatio
	)
	{
		this.FertilityRatio = fertilityRatio;
		this.TerrainType = terrainType;
	}
}


