using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite walkUpSprite;
    public Sprite walkDownSprite;
    public Sprite walkLeftSprite;
    public Sprite walkRightSprite;

    private SpriteRenderer sr;
    private Vector3 lastDirection;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        lastDirection = Vector3.zero;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;

        if (direction == Vector3.zero)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            sr.sprite = idleSprite;
        }
        else
        {
            lastDirection = direction;

            if (Mathf.Abs(x) > Mathf.Abs(z))
            {
                if (x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    sr.sprite = walkRightSprite;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    sr.sprite = walkLeftSprite;
                }
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
                sr.sprite = (z > 0) ? walkUpSprite : walkDownSprite;
            }
        }
    }
}
