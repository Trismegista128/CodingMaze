using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator myCharacterAnimator;
    [SerializeField]
    private Animator myShadowAnimator;
    [SerializeField]
    private ExplodeAnimator myExplosionAnimator;
    [SerializeField]
    private SpriteRenderer myCharacterRenderer;
    [SerializeField]
    private SpriteRenderer myShadowRenderer;

    public void UpdateAnimation(Vector2 movement)
    {
        myCharacterAnimator.SetFloat("Vertical", (int)movement.y);
        myCharacterAnimator.SetFloat("Horizontal", (int)movement.x);
        myCharacterAnimator.SetFloat("Speed", movement.magnitude);
        myShadowAnimator.SetFloat("Speed", movement.magnitude);
    }

    public void TriggerDeath()
    {
        myCharacterAnimator.SetBool("IsDead", true);
        myShadowRenderer.enabled = false;
        myCharacterRenderer.sortingOrder = 0;
    }

    public void TriggerExplosion()
    {
        myExplosionAnimator.TriggerExplosion();
    }

    public void SetSortingOrder(int position)
    {
        myCharacterRenderer.sortingOrder = 100 - position;
    }
    public Sprite GetMySprite => myCharacterRenderer.sprite;
}
