using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexCell : MonoBehaviour{

    public HexCoordinates coordinates;
    public Color color;

	public int population;

    public Terrain terrain;
    public LifeForm unit;
    public Building building;
	public int owner;
    
    // Use this for initialization
    void Start () {
        //Terrain = new Terrain();    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int GetActionRadius()
    {
        return unit.getRadius();
    }

    public void SetActionRadius(int r)
    {
        unit.setRadius(r);
    }

}
