using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
	
public class ContentGenerator: MonoBehaviour {

	public struct Neighbours{
		public HexCell topRight;
		public HexCell topLeft;
		public HexCell middleRight;
		public HexCell middleLeft;
		public HexCell bottomRight;
		public HexCell bottomLeft;

		public Neighbours(HexCell tR, HexCell tL, HexCell mR, HexCell mL, HexCell bR, HexCell bL) {
			topRight = tR;
			topLeft = tL;
			middleRight = mR;
			middleLeft = mL;
			bottomRight = bR;
			bottomLeft = bL;
		}

	}

	public const int NUMBER_OF_CELLS_PER_ROW = 10;
	public const int SELECTED_POPULATION_PER_RACE = 30;
	public const int NUMBER_OF_MAX_UNITS = 4;

	//method that receives the position of a hex cell and returns its neighbours
	Neighbours setInitialNeighbourPositions(HexCell[][] cellMatrix, int i, int j){
		Neighbours neighbours = new Neighbours();
		neighbours.topRight = cellMatrix[i-1][j+1];
		neighbours.topLeft = cellMatrix[i-1][j];
		neighbours.middleRight = cellMatrix[i][j+1];
		neighbours.middleLeft = cellMatrix[i][j-1];
		neighbours.bottomRight = cellMatrix[i+1][j+1];
		neighbours.bottomLeft = cellMatrix[i+1][j];
		return neighbours;
	}

	//assign quadrant
	public int assignQuadrant(int iter, int width, int height){
		bool isInfirstQuadrant = (iter < ((width - 2)) / 2) && (iter < (height-2) / 2);
		bool isInSecondQuadrant = (iter > ((width - 2)) / 2) && (iter < (height - 2) / 2);
		bool isInThirdQuadrant = (iter < ((width - 2)) / 2) && (iter > (height - 2) / 2);
		bool isInForthQuadrant = (iter > ((width - 2)) / 2) && (iter > (height - 2) / 2);

		if(isInfirstQuadrant) return 1;
		else if(isInSecondQuadrant) return 2;
		else if(isInThirdQuadrant) return 3;
		else if(isInForthQuadrant) return 4;

		return 0;
	}

	//generate terrain algorithm
	public void generateTerrain(HexCell[][] cellMatrix){
		int[] owners = new int[4] { 1, 2, 3, 4 };
		int width = Convert.ToInt32(Math.Sqrt(cellMatrix.Length));
		int height = width;
		for (int i = 0; i <= width; i++){
			for (int j = 0; j <= height; j++) {
				int quadrant = this.assignQuadrant (i, width, height);
				cellMatrix[i][j].owner = owners[quadrant];
			}	
		}
	}

	//generate on random
	//use backtrack
	//set flag
	public void generateTerrainRandom(HexCell[] cellMatrix){
		
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
		for (int i = 0; i <= cellMatrix.Length; i++){
			System.Random random = new System.Random();
			int population = (cellMatrix[i].population == 0) ? random.Next(1,NUMBER_OF_MAX_UNITS) : (NUMBER_OF_MAX_UNITS - cellMatrix[i].population);
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
		


