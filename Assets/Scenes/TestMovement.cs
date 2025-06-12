using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;
    private Camera mainCam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;
    }

    void Update()
    {
        // Movement input
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D
        moveInput.y = Input.GetAxisRaw("Vertical");   // W/S
        moveInput.Normalize(); // Ensure consistent diagonal speed

        // Sprite flip based on mouse position
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (mouseWorldPos.x > transform.position.x)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else
        {
            spriteRenderer.flipX = true; // Face left
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
