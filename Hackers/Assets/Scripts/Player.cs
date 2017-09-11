using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public struct GeneratedUnit{
	public int type;
	public int number;
}

public class Player : MonoBehaviour {

	public int generators = 50;

	public int stage = 0;

	Race race = new Race();

	public bool generateChosen;

	// Use this for initialization
	void Start() {
		this.chooseRace();
		this.generateContent();
		this.createBasicUnits();

	}

	// Update is called once per frame
	void Update()
	{

	}

	// this is a general method for turn logic
	public void takeTurn(){
		this.generateOrMutate();
	}

	public void generateOrMutate(){
		if(Input.GetKey(KeyCode.UpArrow)){
			this.generate();
		} else if(Input.GetKey(KeyCode.DownArrow)){
			this.mutate();
		}
	}

	// ACTIONS as described by the doc

	public GeneratedUnit selectUnitType(){
		GeneratedUnit generatedUnit = new GeneratedUnit();

		print ("Please select unit type: 1 or 2");

		if(Input.GetKey(KeyCode.Keypad1)){
			generatedUnit.type = 1;
		} else if(Input.GetKey(KeyCode.Keypad2)){
			generatedUnit.type = 2;
		}

		print ("Now please select how many units");

		generatedUnit.number = KeyboardUtility.detectPressedKey();
		return generatedUnit;
	}

	public void chooseRace(){
		print ("Please select race type: 1, 2 or 3");
		switch (Input.GetKey ()) {
		case KeyCode.Keypad1:
			this.race = 1;
			break;
		case KeyCode.Keypad2:
			this.race = 2;
			break;
		case KeyCode.Keypad3:
			this.race = 3;
			break;
				
		}
	}

	public void createUnits(GeneratedUnit unit){
		
	}

	// generate phase
	public void generate(){
		GeneratedUnit generatedUnit = this.selectUnitType();
		this.createUnits(generatedUnit);
	}

	// mutate phase
	public void mutate(){
		this.stage++;

	}

	public void move(){
		
	}

	public void attack(){
	
	}

	public void takeDefense(){
	
	}

	// alter phase
	public void alter(){}


	// additional growth phase
	public void grow(){}







}