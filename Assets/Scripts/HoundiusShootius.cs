using UnityEngine;
using System.Diagnostics;
using System.CodeDom;

public class HoundiusShootius : Tower
{

    protected override void Awake()
    {
        range = 4f; //this doesnt work - had to set it in unity -> inspector
        fireRate = 2f; //this doesnt work - had to set it in unity -> inspector
        cost = 300; //this doesnt work - had to set it in unity -> inspector
        base.Awake();
    }



    // You can override Shoot() or Update() to customize behavior
    protected override void Shoot(Transform target)
    {
        base.Shoot(target); // or customize the behavior
        //Debug.Log("Ballista fired!");
    }
}