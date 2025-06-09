using UnityEngine;

public class BatHealth : EnemyHealth
{
    private Animator batAnimator;

    protected override void Awake()
    {
        base.Awake(); // Gọi phương thức Awake của lớp cha (EnemyHealth)
        batAnimator = GetComponent<Animator>();
    }

    public override void TakeDamage(float damage)
    {
        if (batAnimator.GetInteger("State") == 1)
        {
            base.TakeDamage(damage); 
        }
    }
}