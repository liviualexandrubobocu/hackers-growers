using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
	
public class ContentGenerator: MonoBehaviour {

	private const int NUMBER_OF_CELLS_PER_PLAYER = 6;

	public HexGrid grid;

	public struct OwnerStartingPosition {
		public int x;
		public int y;
		public int ownerNumber;
	}

	public struct EmptyPositions {
		public bool topLeft;
		public bool topRight;
		public bool bottomLeft;
		public bool bottomRight;
	}

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
	public const int NUMBER_OF_CELLS_OWNED_PER_ROW_BY_PLAYER = 6;
	public const int SELECTED_POPULATION_PER_RACE = 30;
	public const int NUMBER_OF_MAX_UNITS = 4;
	public const int NUMBER_OF_UNITS_BETWEEN_PLAYERS = 3;

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
		

	//add owner to terrain
	public void occupyTerrain(int ownerNumber, ref HexCell[][] cellMatrix, int startingPositionX, int startingPositionY){
		for (int i = startingPositionX; i < startingPositionX + NUMBER_OF_CELLS_PER_PLAYER; i++) {
			for (int j = startingPositionY; j  < startingPositionY + NUMBER_OF_CELLS_PER_PLAYER; j++) {
				if (i < 15 && j < 15) {
					cellMatrix[i][j].owner = ownerNumber;
				}

			}
		}
	}

	//see left quadrants to add players
	public List<OwnerStartingPosition> getRemainingStartingPositions(int width, int height, HexCell[][] cellMatrix){
		List<OwnerStartingPosition> remainingStartingPoints = new List<OwnerStartingPosition>();
		int i = 0;
		int j = 0;
		while(i < height){
			bool notOwnedCondition = ((j < width)
										&& cellMatrix[i][j].owner == 0 
										&& (i + NUMBER_OF_UNITS_BETWEEN_PLAYERS + 1) < height 
										&& (j + NUMBER_OF_UNITS_BETWEEN_PLAYERS + 1 < width) 
										&& cellMatrix[i + NUMBER_OF_UNITS_BETWEEN_PLAYERS + 1][j + NUMBER_OF_UNITS_BETWEEN_PLAYERS + 1].owner == 0);
			if (notOwnedCondition) {
				OwnerStartingPosition ownerStartingPosition = new OwnerStartingPosition();
				ownerStartingPosition.x = i;
				ownerStartingPosition.y = j;
				remainingStartingPoints.Add(ownerStartingPosition);
				j += NUMBER_OF_CELLS_OWNED_PER_ROW_BY_PLAYER + NUMBER_OF_UNITS_BETWEEN_PLAYERS;

			} else {
				j = 0;
				i += NUMBER_OF_CELLS_OWNED_PER_ROW_BY_PLAYER + NUMBER_OF_UNITS_BETWEEN_PLAYERS;
			}
		}
//		foreach(OwnerStartingPosition remainingStartingPoint in remainingStartingPoints){
//			Debug.Log ("x = " + remainingStartingPoint.x);
//			Debug.Log ("y = " + remainingStartingPoint.y);
//		}
		return remainingStartingPoints;
	}

	//check if logged positions are 
	public bool inNexusPosition(int i, int j, int length){
		return (
			(i == 0 && j == 0)
			|| (i == 0 && j == length - 1)
			|| (i == length - 1 && j == 0) 
			|| (i == length - 1 && j == length - 1)
		);
	}


	//generate terrain algorithm
	public void generateTerrain(HexCell[][] cellMatrix){
		int[] owners = new int[4] { 1, 2, 3, 4 };
		int width = Convert.ToInt32(cellMatrix.Length);
		int height = width;

		foreach(int owner in owners){
			List<OwnerStartingPosition> remainingStartingPoints = this.getRemainingStartingPositions (width, height, cellMatrix);
			System.Random random = new System.Random();
			OwnerStartingPosition chosenStartingPosition = new OwnerStartingPosition();
			int chosenPosition = random.Next (0, remainingStartingPoints.Count);
			int i = 0;
			foreach (OwnerStartingPosition position in remainingStartingPoints){
				if (i == chosenPosition) {
					chosenStartingPosition = position;
				}
				i++;	
			}
			this.occupyTerrain(owner, ref cellMatrix, chosenStartingPosition.x, chosenStartingPosition.y);

		}
		for(int k = 0; k < 15; k++){
			string matrix = "";
			for(int j = 0; j < 15; j++){
				matrix += " " + cellMatrix[k][j].owner + " "; 
			}	
			Debug.Log (matrix);
		}
		
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
		this.generateTerrain(grid.cells);

	}

	// Update is called once per frame
	void Update ()
	{

	}


}
		


