using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;

    private Vector2 destination;
    private bool hasDestination = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!hasDestination) return;

        float distance = Vector2.Distance(rb.position, destination);

        if (distance < 0.1f)
        {
            hasDestination = false;
            return;
        }

        Vector2 direction = (destination - rb.position).normalized;

        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    // Call this to set a new destination for the enemy to move toward
    public void MoveTo(Vector2 targetPosition)
    {
        destination = targetPosition;
        hasDestination = true;
    }
}
