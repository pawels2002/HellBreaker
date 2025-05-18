using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{

    Animator animator;
    private Vector3 lastDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        lastDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey("w"))
        {
            animator.SetInteger("Direction", 1);
        } else if (Input.GetKey("s"))
        {
            animator.SetInteger("Direction", 2);
        }
        else if (Input.GetKey("a") || Input.GetKey("d"))
        {
            animator.SetInteger("Direction", 3);
        }
       else
        {
            animator.SetInteger("Direction", 0);
        }*/
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;

        if (direction == Vector3.zero)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            //sr.sprite = idleSprite;
            animator.SetInteger("Direction", 0);
        }
        else
        {
            lastDirection = direction;

            if (Mathf.Abs(x) > Mathf.Abs(z))
            {
                if (x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    //sr.sprite = walkRightSprite;
                    animator.SetInteger("Direction", 3);
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    //sr.sprite = walkLeftSprite;
                    animator.SetInteger("Direction", 3);
                }
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if (z > 0)
                {
                    
                    animator.SetInteger("Direction", 1);
                }
                else
                {
                    
                    animator.SetInteger("Direction", 2);
                }
                //sr.sprite = (z > 0) ? walkUpSprite : walkDownSprite;
            }
        }
    }
}

