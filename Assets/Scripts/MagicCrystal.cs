//using System.CodeDom;
//using System.Diagnostics;
using UnityEngine;

public class MagicCrystal : Tower
{

    protected override void Awake()
    { 
        range = 30f; //this doesnt work - had to set it in unity -> inspector
        fireRate = 2.5f; //this doesnt work - had to set it in unity -> inspector
        cost = 200; //this doesnt work - had to set it in unity -> inspector
        base.Awake();
    }



    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Ballista fired!");
    }
}
