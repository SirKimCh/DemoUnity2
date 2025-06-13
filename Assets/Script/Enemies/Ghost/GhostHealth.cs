using UnityEngine;

public class GhostHealth : EnemyHealth
{
    private GhostManager ghostManager;

    protected override void Awake()
    {
        base.Awake();
        ghostManager = GetComponent<GhostManager>();
    }

    public override void TakeDamage(float damage)
    {
        if (ghostManager != null && !ghostManager.IsVisible())
        {
            return;
        }
        base.TakeDamage(damage);
    }
}