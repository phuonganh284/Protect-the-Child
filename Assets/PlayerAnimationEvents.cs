using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Entity player;

    private void Awake()
    {
        player = GetComponentInParent<Entity>();
    }

    public void DamageTarget()
    {
        player.DamageTarget();
    }

    public void DisableMovementAndJump()
    {
        player.EnableMovementAndJump(false);
    }

    public void EnableMovementAndJump()
    {
        player.EnableMovementAndJump(true);
    }
}
