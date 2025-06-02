using UnityEngine;

public class RotationManager : MonoBehaviour
{
    public Transform player;
    public Transform sprite;
    public Transform[] objectsToRotate;

    private void Awake()
    {
        objectsToRotate = new Transform[transform.childCount];
        for (int i = 0; i < objectsToRotate.Length; i++)
        {
            objectsToRotate[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        foreach (Transform obj in objectsToRotate)
        {
            obj.rotation = player.rotation;
        }
    }
}
