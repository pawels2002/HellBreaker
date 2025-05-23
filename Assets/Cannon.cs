using System.CodeDom;
using System.Diagnostics;
using UnityEngine;

public class Cannon : Tower
{

    protected override void Awake()
    {
        range = 10f;
        fireRate = 0.5f;
        cost = 50;
        base.Awake();
    }



    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Ballista fired!");
    }
}
