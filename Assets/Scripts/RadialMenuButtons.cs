
using UnityEngine;
using UnityEngine.UI;
public class RadialMenuButtons : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

}
