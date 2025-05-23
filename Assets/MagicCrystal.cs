//using System.CodeDom;
//using System.Diagnostics;
using UnityEngine;

public class MagicCrystal : Tower
{

    protected override void Awake()
    { 
        range = 30f;
        fireRate = 2.5f;
        cost = 200;
        base.Awake();
    }



    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Ballista fired!");
    }
}
