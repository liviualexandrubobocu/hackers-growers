using UnityEngine;
using System.Collections;

public class HexCell : MonoBehaviour {

    public HexCoordinates coordinates;
    public Color color;

	public int population;

    public Terrain terrain;
    public LifeForm unit;
    public Building building;
	public Player owner;
    
    // Use this for initialization
    void Start () {
        //Terrain = new Terrain();    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
