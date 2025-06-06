using UnityEngine;
using System.Diagnostics;
using System.CodeDom;

public class Ballista : Tower
{
    protected override void Awake()
    {
        range = 4f; 
        fireRate = 1.5f; 
        cost = 100; 
        level = 0;
        upgradeCost = 100; 
        base.Awake();
    }

   // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Ballista fired!");
    }
}
