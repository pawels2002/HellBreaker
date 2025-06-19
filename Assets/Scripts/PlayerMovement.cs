using System.Collections;
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

    public float attackCooldown = 0.5f; 
    public float lastAttackTime = -Mathf.Infinity;
    float attackRadius = 1.5f; 

    float attackAngle = 40f;
    private int currentHP;
    private bool isInvulnerable = false;
    private float invulnerableTime = 1f;
    private float invulnerableTimer = 0f;

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
    private Renderer[] renderers;
    private Color originalColor;
    private bool isModelGreyed = false;
    private bool isAttacking = false;
    private Vector3 lastAttackDir = Vector3.forward;
    private Vector3 originalLocalScale;
    private bool IsDead => HealthPlayer.Instance.GetHealth() <= 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fixedY = transform.position.y;
        targetRotation = transform.rotation;
        currentHP = HealthPlayer.Instance.GetHealth();

        // Cache all renderers
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            originalColor = renderers[0].material.color;
        }
        originalLocalScale = transform.localScale;
    }
    private enum AttackDirection { Left, Right, Top, Bottom }
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (!isAttacking)
            {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            inputDirection = new Vector3(x, 0, z).normalized;
            }
            else
            {
                inputDirection = Vector3.zero;
            }
            Vector3 spawnPos = transform.position + transform.forward * 2f;
            spawnPos.y = 1f;

            if (isInvulnerable)
            {
                invulnerableTimer -= Time.deltaTime;
                if (invulnerableTimer <= 0f)
                {
                    isInvulnerable = false;
                    SetPlayerCollision(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
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
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                buildSystem.BuildTower(spawnPos);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                Money.Instance.AddMoney(1000);
            }
        }
        if (!IsDead && !isAttacking && Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Vector3 attackDir = GetAttackDirectionFromMouse();
            StartCoroutine(AttackRoutine(attackDir));
            lastAttackTime = Time.time;
        }


        // At the end of Update()
        if (IsDead && !isModelGreyed)
        {
            SetModelGray(true);
        }
        else if (!IsDead && isModelGreyed)
        {
            SetModelGray(false);
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
    private Vector3 GetAttackDirectionFromMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        float enter;
        if (playerPlane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dir = (hitPoint - transform.position).normalized;
            if (dir.sqrMagnitude > 0.01f)
                lastAttackDir = dir;
            return lastAttackDir;
        }
        // Default to forward if raycast fails
        return lastAttackDir;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!isInvulnerable && collision.gameObject.GetComponent<Enemy>() != null)
        {
            TakeDamage(10); // or any damage value you want
            isInvulnerable = true;
            invulnerableTimer = invulnerableTime;
        }
        if (HealthPlayer.Instance.GetHealth() <= 0)
        {

            SetPlayerCollision(false);
        }
    }

    private void TakeDamage(int amount)
    {
        HealthPlayer.Instance.RemoveHealth(amount);
    }

    private void SetPlayerCollision(bool enabled)
    {
        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null)
        {
            playerCollider.enabled = enabled;
        }
    }
    private IEnumerator AttackRoutine(Vector3 attackDir)
    {
        isAttacking = true;

        // Determine animation direction
        float animAngle = Mathf.Atan2(attackDir.x, attackDir.z) * Mathf.Rad2Deg;
        int animDirection = 1;
        bool shouldFlip = false;

        if (animAngle >= -45f && animAngle < 45f) //up
            animDirection = 1;
        else if (animAngle >= 45f && animAngle < 135f) //right
            animDirection = 3;
        else if (animAngle >= -135f && animAngle < -45f) //left
            animDirection = 3;
        else //down
            animDirection = 2;


        if (animDirection == 3) // Side attack
        {
            if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.z) && Mathf.Abs(inputDirection.x) > 0.01f)
            {
                // Moving right and attacking left, or moving left and attacking right
                if ((inputDirection.x > 0 && attackDir.x < 0) || (inputDirection.x < 0 && attackDir.x > 0))
                    shouldFlip = true;
            }
            else
            {
                // Idle: flip if attacking left
                if (attackDir.x < 0)
                    shouldFlip = true;
            }
        }
        Vector3 localScale = transform.localScale;
        localScale.x = shouldFlip ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
        transform.localScale = localScale;

        if (_animator != null)
        {
            _animator.SetInteger("attackDirection", animDirection);
            _animator.SetBool("isAttacking", true);
        }

        DoAttackLogic(attackDir);

        yield return new WaitForSeconds(0.4f);

        if (_animator != null)
        {
            _animator.SetBool("isAttacking", false);
        }
        
        isAttacking = false;
        transform.localScale = originalLocalScale;

    }
    private void DoAttackLogic(Vector3 attackDir)
    {
     float angle = attackAngle;
    Vector3 attackOrigin = transform.position + Vector3.up * 1f;

    Collider[] hits = Physics.OverlapSphere(attackOrigin, attackRadius);
    bool hitAny = false;
    foreach (var hit in hits)
    {
        Enemy enemy = hit.GetComponent<Enemy>();
        if (enemy != null)
        {
            Vector3 toTarget = (hit.transform.position - attackOrigin).normalized;
            float targetAngle = Vector3.Angle(attackDir, toTarget);
            if (targetAngle <= angle)
            {
                Destroy(enemy.gameObject);
                Money.Instance.AddMoney(5);
                hitAny = true;
            }
        }
    }

    // Debug: Draw the cone area in Scene view
    Debug.DrawRay(attackOrigin, Quaternion.Euler(0, angle, 0) * attackDir * attackRadius, Color.red, 0.2f);
    Debug.DrawRay(attackOrigin, Quaternion.Euler(0, -angle, 0) * attackDir * attackRadius, Color.red, 0.2f);
    Debug.DrawRay(attackOrigin, attackDir * attackRadius, Color.red, 0.2f);

    if (!hitAny)
        Debug.Log("No enemy hit.");
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
    private void SetModelGray(bool gray)
    {
        if (renderers == null) return;
        foreach (var rend in renderers)
        {
            if (gray)
                rend.material.color = Color.gray;
            else
                rend.material.color = originalColor;
        }
        isModelGreyed = gray;
    }
}


