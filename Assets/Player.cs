using UnityEngine;

public class Player : Entity
{
    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8;

    [Header("Dash details")]
    [SerializeField] protected float dashForce = 25;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 0.8f;
    private bool isDashing;
    private bool canDash = true;
    private float dashTimer;
    private float originalGravity;

    [Header("Strong Attack")]
    [SerializeField] private float strongAttackCooldown = 1.5f;
    [SerializeField] private float strongAttackDuration = 0.4f;
    private bool canStrongAttack = true;
    private bool isAttackingStrong;

    private bool canJump = true;
    private float xInput;

    protected override void Awake()
    {
        base.Awake();
        originalGravity = rb.gravityScale;
    }
    protected override void Update()
    {
        HandleInput();
        HandleDash();
        base.Update();
    }


    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            TryToJump();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleAttack();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            TryStrongAttack();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryToDash();
        }
    }

    private void TryStrongAttack()
    {
        if (!canStrongAttack || isDashing)
            return;

        currentDamage = 3;

        isAttackingStrong = true;
        canStrongAttack = false;

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        EnableMovementAndJump(false);

        anim.SetTrigger("strongAttack");

        Invoke(nameof(EndStrongAttack), strongAttackDuration);
        Invoke(nameof(ResetStrongAttack), strongAttackCooldown);
    }

    private void EndStrongAttack()
    {
        isAttackingStrong = false;
        EnableMovementAndJump(true);
    }

    private void ResetStrongAttack()
    {
        canStrongAttack = true;
    }

    protected override void HandleAttack()
    {
        if (isGrounded && !isAttackingStrong && !isDashing)
        {
            currentDamage = 1;
            anim.SetTrigger("attack");
        }
    }
    protected override void HandleMovement()
    {
        if (isDashing)
            return;

        if (isAttackingStrong)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (canMove)
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void HandleDash()
    {
        if (!isDashing)
            return;

        rb.linearVelocity = new Vector2(facingDir * dashForce, rb.linearVelocity.y);

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0)
        {
            isDashing = false;
            rb.gravityScale = originalGravity;

            Invoke(nameof(ResetDash), dashCooldown);
        }
    }
    private void ResetDash()
    {
        canDash = true;
    }
    private void TryToDash()
    {
        if (!canDash)
            return;

        isDashing = true;
        canDash = false;
        dashTimer = dashDuration;

        rb.gravityScale = 0;
        anim.SetTrigger("dash");
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public override void EnableMovementAndJump(bool enable)
    {
        base.EnableMovementAndJump(enable);
        canJump = enable;
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }

}
