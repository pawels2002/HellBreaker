using System.ComponentModel.Design.Serialization;
using System.Reflection.Emit;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Animator _animator;

    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float rotationStep = 90f;

    //public GameObject ballistaPrefab;
    //public GameObject cannonPrefab;
    //public GameObject magicCrystalPrefab;
    //public Transform towerSpawnPoint; // Optional, or use transform.position

    private Rigidbody rb;
    private Vector3 inputDirection;
    private float fixedY;

    [SerializeField] private Build buildSystem;
    
    private Quaternion targetRotation;
    [SerializeField] private GameObject radialBuildMenu;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fixedY = transform.position.y;
        targetRotation = transform.rotation;
        //transform.rotation = Quaternion.Euler(45f, 0f, 0f); //should be done but walks inside grass
        //buildSystem = FindObjectOfType<Build>();
    }   

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            inputDirection = new Vector3(x, 0, z).normalized;
            Vector3 spawnPos = transform.position + transform.forward * 2f; // 2 units in front
            spawnPos.y = 1f;
            if (Input.GetKeyDown(KeyCode.B))
            {
                //if radialBuildMenu is active, else
                if (radialBuildMenu.activeSelf)
                {
                    radialBuildMenu.SetActive(false);
                    Time.timeScale = 1.0f;
                }
                else
                {
                    radialBuildMenu.SetActive(true);
                    Time.timeScale = 0.5f;
                }
                //radialBuildMenu.SetActive(!radialBuildMenu.activeSelf);
                //Time.timeScale = 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                buildSystem.BuildTower(spawnPos);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                Money.Instance.AddMoney(1000);  //added a cheat code, so you can test the towers without grinding for money
            }
            //since we dont care about rotating camera i commented this out
            /*
            if (Input.GetKeyDown(KeyCode.Q))
            {
                targetRotation *= Quaternion.Euler(0f, -rotationStep, 0f);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                targetRotation *= Quaternion.Euler(0f, rotationStep, 0f);
            }
            */

            //-----ONLY FOR TESTING------
            /*
            if (Input.GetKeyDown(KeyCode.F))
            {
                Vector3 spawnPos = transform.position + transform.forward * 2f; // 2 units in front
                spawnPos.y = 1f;
                Instantiate(ballistaPrefab, spawnPos, Quaternion.identity);
                Debug.Log("Tower spawned!");
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                Vector3 spawnPos = transform.position + transform.forward * 2f; // 2 units in front
                spawnPos.y = 1.5f;
                Instantiate(cannonPrefab, spawnPos, Quaternion.identity);
                Debug.Log("Tower spawned!");
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Vector3 spawnPos = transform.position + transform.forward * 2f; // 2 units in front
                spawnPos.y = 2f;
                Instantiate(magicCrystalPrefab, spawnPos, Quaternion.identity);
                Debug.Log("Tower spawned!");
            }
            */
            //-----ONLY FOR TESTING------\\
        }
    }

    void FixedUpdate()
    {

        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

        Vector3 moveDir = transform.TransformDirection(inputDirection) * moveSpeed;
        rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);

        Vector3 pos = rb.position;
        pos.y = fixedY;
        rb.position = pos;
    }
}
