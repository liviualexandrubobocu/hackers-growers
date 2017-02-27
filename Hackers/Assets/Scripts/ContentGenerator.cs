using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	
public class ContentGenerator: MonoBehaviour {

	public struct Neighbours{
		public int topRight;
		public int topMiddle;
		public int topLeft;
		public int middleRight;
		public int middleLeft;
		public int bottomRight;
		public int bottomMiddle;
		public int bottomLeft;

		public Neighbours(int tR, int tM, int tL, int mR, int mL, int bR, int bM, int bL) {
			topRight = tR;
			topMiddle = tM;
			topLeft = tL;
			middleRight = mR;
			middleLeft = mL;
			bottomRight = bR;
			bottomMiddle = bM;
			bottomLeft = bL;
		}

	}

	public const int NUMBER_OF_CELLS_PER_ROW = 10;
	public const int SELECTED_POPULATION_PER_RACE = 30;
	public const int NUMBER_OF_MAX_UNITS = 4;
	public const int MATRIX_LENGTH = 100;

	//method that receives the position of a hex cell and returns its neighbours
	Neighbours setInitialNeighbourPositions(int position){
		Neighbours neighbours = new Neighbours();
		neighbours.topRight = position - NUMBER_OF_CELLS_PER_ROW - 1;
		neighbours.topMiddle = position - NUMBER_OF_CELLS_PER_ROW;
		neighbours.topLeft = position - NUMBER_OF_CELLS_PER_ROW + 1;
		neighbours.middleRight = position + 1;
		neighbours.middleLeft = position - 1;
		neighbours.bottomRight = position + NUMBER_OF_CELLS_PER_ROW + 1;
		neighbours.bottomMiddle = position + NUMBER_OF_CELLS_PER_ROW;
		neighbours.bottomLeft = position + NUMBER_OF_CELLS_PER_ROW - 1;
		return neighbours;
	}


	//Set a set of numbers { 0..3 }
	//For Each position in the terrain 
	//Set number as random number from set;
	//If Adding a random number isn’t bigger than total number of Units for player
	//	Add Units in Units Matrix
	//	Else 
	//	Remove random number from set
	//	Use recursion to check the sum again until there is no other element

	//	If number of units set per player is greater than the number of the units set on the map
	//	Retake algorithm 


	//random selection and recreate total
	public int generatePopulation(int totalPopulation, HexCell[] cellMatrix){
		for (int i = 0; i <= MATRIX_LENGTH; i++){
			System.Random random = new System.Random();
			int population = (cellMatrix[i].population == 0) ? NUMBER_OF_MAX_UNITS : (NUMBER_OF_MAX_UNITS - cellMatrix[i].population);
			cellMatrix[i].population += population;
			totalPopulation += population;
		}

		return totalPopulation;
	}


	//recursive random generation

	public void generateRandomUnits(int totalPopulation, HexCell[] cellMatrix){
		int grownPopulation = this.generatePopulation(totalPopulation, cellMatrix);
		if (grownPopulation < SELECTED_POPULATION_PER_RACE) {
			this.generateRandomUnits(grownPopulation, cellMatrix);
		}
	}


	// Use this for initialization
	void Start () {


	}

	// Update is called once per frame
	void Update ()
	{

	}


}
		


