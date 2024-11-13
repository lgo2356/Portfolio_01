using UnityEngine;

public class Boss : Monster
{
    public void SetTaunt()
    {
        animator.SetTrigger("DoTaunt");
    }
}
