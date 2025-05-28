using UnityEngine;

public class ImpAnimationScript : MonoBehaviour
{
    private Animator impAnimator;
    private SpriteRenderer spriteRenderer;
    private Enemy enemy;

    void Start()
    {
        impAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (impAnimator != null && impAnimator.isActiveAndEnabled && enemy != null)
        {
            Vector3 direction = enemy.CurrentDirection;

            if (direction == Vector3.zero)
            {
                impAnimator.SetInteger("dir", 0);
                spriteRenderer.flipX = false;
            }
            else
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    impAnimator.SetInteger("dir", 1); // Horizontal
                    spriteRenderer.flipX = direction.x < 0;
                }
                else
                {
                    impAnimator.SetInteger("dir", direction.z > 0 ? 2 : 0); // Up or down
                    spriteRenderer.flipX = false;
                }
            }
        }
    }
}
