using UnityEngine;

public class ExplodeAnimator : MonoBehaviour
{
    public PlayerAnimations MyCharacterAnimations;
    public Animator MyExplosionAnimator;

    // Start is called before the first frame update

    public void TriggerExplosion()
    {

        MyExplosionAnimator.SetTrigger("Explode");
    }

    public void OnReadyToChangeSprite()
    {
        MyCharacterAnimations.TriggerDeath();
    }

    public void OnExplosionFinished()
    {

    }
}
