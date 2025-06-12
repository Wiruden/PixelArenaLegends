using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float attackDuration = 1;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    public Collider2D attackHitbox;
    public static PlayerController Instance;
    [SerializeField] public int PlayerHealth = 10;

    public GemManager cm;


    private bool isAttacking = false;

    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked. isAttacking: " + isAttacking);
        }
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartAttack();
        }
        PlayerInput();
        AdjustPlayerFacingDirection();
        Move();
    }
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;

        }
        else
        {
            mySpriteRender.flipX = false;
        }
        bool facingLeft = mousePos.x < playerScreenPoint.x;
        if (attackHitbox is BoxCollider2D boxCollider)
        {
            Vector2 offset = boxCollider.offset;
            offset.x = Mathf.Abs(offset.x) * (facingLeft ? -1 : 1);
            boxCollider.offset = offset;
        }
    }

    private void PlayerInput()
    {
        if (isAttacking)
        {
            movement = Vector2.zero;
            myAnimator.SetFloat("moveX", 0);
            myAnimator.SetFloat("moveY", 0);
            return;
        }
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (isAttacking) return;
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    void Start()
    {
        if (attackHitbox != null)
            attackHitbox.enabled = false;
    }

    public void EnableHitbox()
    {
        attackHitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        attackHitbox.enabled = false;
    }
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        myAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDuration); // e.g. 0.5f
        isAttacking = false;
    }
    private void StartAttack()
    {
        if (!isAttacking)
            StartCoroutine(AttackRoutine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Gem"))
        {
            Destroy(other.gameObject);
            cm.gemCount++;
        }
    }

}
