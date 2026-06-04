using System.Collections;
using UnityEngine;

public class Move
{
    public moveBase mbase { get; set; }
    public int Point { get; set; }

    public Move(moveBase mobase)
    {
        mbase = mobase;
        Point = mbase.Point;
    }

}
