using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class HeroMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Vector2 input;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public Vector2 FacingDirection { get; private set; } = Vector2.right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetInput(Vector2 movementInput)
    {
        input = movementInput.normalized;

        if (input != Vector2.zero)
            FacingDirection = input;

        anim.SetFloat("moveX", input.x);
        anim.SetFloat("moveY", input.y);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * (moveSpeed * Time.fixedDeltaTime));
    }

    public void UpdateFacingToMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = (mouseWorldPos - transform.position);

        if (direction.sqrMagnitude > 0.01f)
            FacingDirection = direction.normalized;

        spriteRenderer.flipX = FacingDirection.x < 0;
    }
}
