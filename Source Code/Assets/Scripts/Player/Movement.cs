using UnityEngine;

public class Movement : MonoBehaviour
{
    // [SerializeField] allows us to change value in Unity.  
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private float wallJumpCD;
    private float horizontalInput;
   
    private void Awake()
    {
        // Refers to the components in the object.
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Flips model left/right depending on input.
        if (horizontalInput > 0.1f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Sets Animator parameters
        animator.SetBool("Run", horizontalInput != 0); // Checks if directional keys are being pressed. If nothing is being pressed, input = 0, therefore player will not be moving.
        animator.SetBool("Grounded", isGrounded());

        // Wall jump code
        if (wallJumpCD > 0.2f)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

            if (isonWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }

            else
                body.gravityScale = 2;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCD += Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        animator.SetTrigger("Jump");
        }
       
        else if (isonWall() && !isGrounded())
        {
            if (horizontalInput == 0) 
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 5, 1); // Jump away from the wall.
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flips the player when jumping away from the wall.
            }
            
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 6); // Climbing the wall.

            wallJumpCD = 0;

        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null; // Checks whats under the player.
    }

    private bool isonWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack() // Checks conditions if the player can attack or not.
    {
        return horizontalInput == 0 && isGrounded() && !isonWall();
    }

}