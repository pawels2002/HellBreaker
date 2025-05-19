using System.Diagnostics;
using UnityEngine;

public class Cannon : Tower
{

    protected override void Awake()
    {
        base.Awake();
        range = 5f;
        fireRate = 0.5f;
    }



    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Ballista fired!");
    }
}
