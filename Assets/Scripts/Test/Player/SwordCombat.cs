using UnityEngine;
using System.Collections;

public class SwordCombat : HeroCombatBase
{
    [SerializeField] private Collider2D attackHitbox;
    [SerializeField] private float attackDuration = 0.5f;

    private bool isAttacking = false;
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        if (attackHitbox != null)
            attackHitbox.enabled = false;
    }

    public override void OnAttack()
    {
        if (!isAttacking)
            StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");

        if (attackHitbox is BoxCollider2D box)
        {
            Vector2 offset = box.offset;
            offset.x = Mathf.Abs(offset.x) * (movement.FacingDirection.x < 0 ? -1 : 1);
            box.offset = offset;
        }

        attackHitbox.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        attackHitbox.enabled = false;
        isAttacking = false;
    }

}
