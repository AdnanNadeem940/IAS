using UnityEngine;

public class Move
{
    public Bolt fromScrew;

    public Bolt toScrew;

    public int number;

    public Move(Bolt from = null, Bolt to = null, int num = 0)
    {
        this.fromScrew = from;
        this.toScrew = to;
        this.number = num;
    }
}
