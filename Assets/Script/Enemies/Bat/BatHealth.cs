using UnityEngine;

public class BatHealth : EnemyHealth
{
    private Animator batAnimator;

    protected override void Awake()
    {
        base.Awake();
        batAnimator = GetComponent<Animator>();
    }

    public override void TakeDamage(float damage)
    {
        if (batAnimator.GetBool("IsMove"))
        {
            base.TakeDamage(damage); 
        }
    }
}