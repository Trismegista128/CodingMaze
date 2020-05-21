using UnityEngine;

public class ExplodeAnimator : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimations characterAnimator;
    [SerializeField]
    private Animator myExplosionAnimator;

    public void TriggerExplosion()
    {
        myExplosionAnimator.SetTrigger("Explode");
    }

    public void OnReadyToChangeSprite()
    {
        characterAnimator.TriggerDeath();
    }
}
