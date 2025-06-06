using UnityEngine;
using System.Diagnostics;
using System.CodeDom;

public class Catapult : Tower
{
    protected override void Awake()
    {
        range = 10f; 
        fireRate = 0.5f; 
        cost = 150;
        level = 0;
        upgradeCost = 150;
        base.Awake();
    }

    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Catapult fired!");
    }
}