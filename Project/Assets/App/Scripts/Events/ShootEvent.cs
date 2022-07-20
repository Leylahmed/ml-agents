using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicBox.EventManagement;

public class ShootEvent : GameEvent
{
    public int TeamID;

    public ShootEvent (int teamID)
    {
        TeamID = teamID;
    }
}
