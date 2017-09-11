using System;

namespace AssemblyCSharp
{
	public struct Neighbour{
		public int x;
		public int y;
	}

	public class Neighbours
	{
		public Neighbour topRight;
		public Neighbour topLeft;
		public Neighbour middleRight;
		public Neighbour middleLeft;
		public Neighbour bottomRight;
		public Neighbour bottomLeft;


		public Neighbours(){}

		public static Neighbour getTopLeft(int i, int j, HexCell[][] cellMatrix){
			Neighbour neighbour = new Neighbour ();
			if (i % 2 == 0) {
				if (i - 1 >= 0 && j + 1 < cellMatrix.Length) {
					neighbour.x = i - 1;
					neighbour.y = j + 1;
				}
			} else {
				if (i - 1 >= 0 && j < cellMatrix.Length) {
					neighbour.x = i - 1;
					neighbour.y = j;
				}
			}
		}

		public static Neighbour getTopRight(int i, int j, HexCell[][] cellMatrix){
			Neighbour neighbour = new Neighbour();
			if (i % 2 == 0) {
				if (i - 1 >= 0 && j + 1 < cellMatrix.Length) {
					neighbour.x = i - 1;
					neighbour.y = j + 1;
				}
			} else {
				neighbour.x = 1;
				neighbour.y = 2;
			}
			return neighbour;
		}

		public static Neighbour getMiddleLeft(int i, int j, HexCell[][] cellMatrix){
			Neighbour neighbour = new Neighbour ();
			if (i % 2 == 0) {
				if (i - 1 >= 0 && j + 1 < cellMatrix.Length) {
					neighbour.x = i - 1;
					neighbour.y = j + 1;
				}
			} else {

			}
		}

		public static Neighbour getMiddleRight(int i, int j, HexCell[][] cellMatrix){
			Neighbour neighbour = new Neighbour ();
			if (i % 2 == 0) {
				if (i - 1 >= 0 && j + 1 < cellMatrix.Length) {
					neighbour.x = i - 1;
					neighbour.y = j + 1;
				}
			} else {

			}
		}

		public static Neighbour getBottomLeft(int i, int j, HexCell[][] cellMatrix){
			Neighbour neighbour = new Neighbour ();
			if (i % 2 == 0) {
				if (i - 1 >= 0 && j + 1 < cellMatrix.Length) {
					neighbour.x = i - 1;
					neighbour.y = j + 1;
				}
			} else {

			}
		}

		public static Neighbour getBottomRight(int i, int j, HexCell[][] cellMatrix){
			Neighbour neighbour = new Neighbour ();
			if (i % 2 == 0) {
				if (i - 1 >= 0 && j + 1 < cellMatrix.Length) {
					neighbour.x = i - 1;
					neighbour.y = j + 1;
				}
			} else {

			}
		}

		public static Neighbours getAllNeighbours(int i, int j, HexCell[][] cellMatrix){
			this.topLeft = Neighbours.getTopLeft();
			this.topRight = Neighbours.getTopRight();
			this.middleLeft = Neighbours.getMiddleLeft();
			this.middleRight = Neighbours.getMiddleRight();
			this.bottomLeft = Neighbours.getBottomLeft();
			this.bottomRight = Neighbours.getBottomRight();
		}
	}
}

