using UnityEngine;

public abstract class HeroCombatBase : MonoBehaviour
{
    protected HeroMovement movement;

    protected virtual void Awake()
    {
        movement = GetComponent<HeroMovement>();
    }

    public abstract void OnAttack();
}