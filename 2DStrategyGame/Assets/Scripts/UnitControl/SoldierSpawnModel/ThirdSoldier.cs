using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdSoldier : Soldier
{
    public override void Initialize(int health, int id, int damage, float attackDelay)
    {
        Health = health;
        SoldierID = id;
        Damage = damage;
        AttackDelay = attackDelay;
    }
    public override IEnumerator Attack(GameObject targetObject)
    {
        yield return base.Attack(targetObject);
    }


}
