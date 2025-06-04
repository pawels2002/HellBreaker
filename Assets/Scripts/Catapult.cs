using UnityEngine;
using System.Diagnostics;
using System.CodeDom;

public class Catapult : Tower
{

    protected override void Awake()
    {
        range = 10f; //this doesnt work - had to set it in unity -> inspector
        fireRate = 0.5f; //this doesnt work - had to set it in unity -> inspector
        cost = 150; //this doesnt work - had to set it in unity -> inspector
        base.Awake();
    }



    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Ballista fired!");
    }
}