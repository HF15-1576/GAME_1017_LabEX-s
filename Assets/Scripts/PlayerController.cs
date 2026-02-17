using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    //Movement Parameters or like Values for movement and jumping
    [SerializeField] private float forwardSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private bool canMove;
    private bool isGrounded;

    // We use Awake to initialize our Rigidbody2D reference and set it up for 2D platformer movement
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // We use FixedUpdate for physics-based movement to ensure consistent behavior regardless of frame rate
    private void FixedUpdate()
    {
        if (!canMove) return;

        // Only control X so gravity can handle Y
        rb.linearVelocity = new Vector2(forwardSpeed, rb.linearVelocity.y);
    }

    // We use Update to check for player input for jumping and other actions that don't require physics calculations
    private void Update()
    {
        if (!canMove) return;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            isGrounded = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.K))
            GameManager.Instance.Die();
    }

    // Ground detection with collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!canMove) rb.linearVelocity = Vector2.zero;
    }

    public void ResetPlayer(Vector3 pos, Quaternion rot)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.SetPositionAndRotation(pos, rot);
    }
}
