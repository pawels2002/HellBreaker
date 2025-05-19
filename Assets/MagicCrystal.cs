using System.Diagnostics;
using UnityEngine;

public class MagicCrystal : Tower
{

    protected override void Awake()
    {
        base.Awake();
        range = 20f;
        fireRate = 0.25f;
    }



    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Ballista fired!");
    }
}
