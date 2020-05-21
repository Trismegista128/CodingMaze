using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator MyAnimation;
    public Animator MyShadow;
    public SpriteRenderer MyRenderer;
    public SpriteRenderer MyShadowRenderer;
    public void UpdateAnimation(Vector2 movement)
    {
        MyAnimation.SetFloat("Vertical", (int)movement.y);
        MyAnimation.SetFloat("Horizontal", (int)movement.x);
        MyAnimation.SetFloat("Speed", movement.magnitude);
        MyShadow.SetFloat("Speed", movement.magnitude);
    }

    public void TriggerDeath()
    {
        MyAnimation.SetBool("IsDead", true);
        MyShadowRenderer.enabled = false;
        MyRenderer.sortingOrder = 0;
    }

    public Sprite GetMySprite => MyRenderer.sprite;
}
