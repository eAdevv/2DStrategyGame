using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondSoldier : Soldier
{
    public override void Initialize(int health, int id, int damage, float attackRate)
    {
        this.Health = health;
        this.SoldierID = id;
        this.Damage = damage;
        this.AttackRate = attackRate;


        healthText.text = Health.ToString();
    }


    public override IEnumerator Attack(GameObject targetObject)
    {
        //
        //
        Debug.Log("COMMANDO ATTACKING");
        return base.Attack(targetObject);
    }
}
