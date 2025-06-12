using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public GemManager gemManager;
    private HeroMovement movement;
    private HeroCombatBase combat;

    private void Awake()
    {
        movement = GetComponent<HeroMovement>();
        combat = GetComponent<HeroCombatBase>();
    }

    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.SetInput(input);

        movement.UpdateFacingToMouse();

        if (Input.GetMouseButtonDown(0))
        {
            combat.OnAttack();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gem"))
        {
            gemManager.AddGem();
            Destroy(other.gameObject);
        }
    }
}
