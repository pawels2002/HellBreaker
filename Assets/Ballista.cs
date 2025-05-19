using UnityEngine;
using System.Diagnostics;

public class Ballista : Tower
{

    protected override void Awake()
    {
        base.Awake();
        range = 10f;
        fireRate = 1f;
    }
    


    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Ballista fired!");
    }
}
