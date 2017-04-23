using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool IsValid(int xLimit, int yLimit)
    {
        return (0 <= X) && (X < xLimit) && (0 <= Y) && (Y < yLimit);
    }

    public override bool Equals(object obj)
    {
        var pos = obj as Position;
        if (pos == null)
            return false;

        return X == pos.X && Y == pos.Y;
    }

    public override int GetHashCode()
    {
        int hash = 23;
        hash = hash * 31 + X;
        hash = hash * 31 + Y;
        return hash;
    }
}

public class JsonLifeform
{
    public string LifeType { get; set; }
	public int LifeSpan { get; set; }
    public int GrowingRate { get; set; }
    public int AttackPower { get; set; }
    public int DefensePower { get; set; }
    public int MutationTime { get; set; }
}

public class Lifeforms
{
    public Dictionary<string, JsonLifeform> carbonBased;
}