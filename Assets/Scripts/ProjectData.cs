using System;
using System.Collections.Generic;

[Serializable]
public class ProjectData
{
    public float hearts;
    public int peach;
    public bool canJump;
    public bool canDoubleJump;
    public bool canDash;
    public bool canAttack;
    public List<string> gameObjectsNames;
    public List<float> player_coordinates;
}
