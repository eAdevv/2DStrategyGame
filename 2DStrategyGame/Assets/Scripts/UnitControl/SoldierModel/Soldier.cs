using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Soldier : MonoBehaviour
{

    public int Health { get; set; }
    public int SoldierID { get; set; }
    public int Damage { get; set; }

    public void Initialize(int health, int id, int damage)
    {
        Health = health;
        SoldierID = id;
        Damage = damage;
    }
    
}
