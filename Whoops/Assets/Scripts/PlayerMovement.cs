using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 dustPos;
    [SerializeField] private GameObject dustPS;

    [SerializeField] private float fallDamageThreshold;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isJumping;
    private float moveHorivontal;
    private float maxYVel;

    private GameObject childObject;
    public bool slimed = false;

    // Start is called before the first frame update
    void Start()
    {
        dustPos = new Vector2(transform.position.x, transform.position.y - 1);
        childObject = Instantiate(dustPS, dustPos, Quaternion.identity) as GameObject;
        childObject.transform.parent = gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorivontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            maxYVel = 0;
            childObject.SetActive(false);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            StartCoroutine(MoreGravity());
        }

        if (isJumping)
        {
            float x = 2;
            if(rb.velocity.y < maxYVel)
            {
                maxYVel = rb.velocity.y;
                x -= 0.25f;
                transform.localScale = new Vector3(x, 2, 1); 
            }
        }
    }

    private void FixedUpdate()
    {
        if (moveHorivontal > 0.1f || moveHorivontal < -0.1f)
        {
            rb.AddForce(new Vector2(moveHorivontal * moveSpeed, 0f), ForceMode2D.Impulse);
            if(moveHorivontal > 0.1f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 5));
            }
            else if(moveHorivontal < -0.1f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -5));
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("BellowWater"))
        {
            maxYVel = 0;
            StartCoroutine(KayoteTime());
        }
        else if (collision.CompareTag("Slime"))
        {
            moveSpeed = 4;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("BellowWater"))
        {
            slimed = false;
            childObject.SetActive(true);
            transform.localScale = new Vector3(2, 2, 1);
            rb.gravityScale = 10;
            isJumping = false;
            Debug.Log(maxYVel);
            if (maxYVel <= -fallDamageThreshold && collision.CompareTag("Ground"))
            {
                Debug.Log(maxYVel);
                gameObject.GetComponent<Player>().TakeDamage(-maxYVel);
            }
        }
        else if (collision.CompareTag("Slime"))
        {
            Debug.Log("Slime)");
            isJumping = false;
            slimed = true;
            rb.gravityScale = 10;
            moveSpeed = 1.5f;
        }
    }
    IEnumerator MoreGravity()
    {
        yield return new WaitForSeconds(0.5f);
        while (isJumping)
        {
            rb.gravityScale += 1;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator KayoteTime()
    {
        yield return new WaitForSeconds(0.1f);
        isJumping = true;
    }
}
