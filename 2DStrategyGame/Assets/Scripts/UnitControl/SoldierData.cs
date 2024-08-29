using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewSoldierData" , menuName = "Soldiers / SoldierData")]
public class SoldierData : ScriptableObject
{
    public int Health;
    public int Damage;
    public int ID;
    public float AttackRate;
}
