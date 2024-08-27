using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager 
{
    public static Action OnSetBuildInformation;
    public static Action<GameObject, SoldierData,Vector3> OnSoldierSpawner;

}
