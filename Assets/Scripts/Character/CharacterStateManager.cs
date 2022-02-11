using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    Character character;
    LayerMask ground;
    [HideInInspector] public bool justJumped; //need this so that it doesn't throw a jab while jumping if jump and attack are pressed at the same time
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isRunning;

    private void Start()
    {
        character = GetComponent<Character>();
        ground = LayerMask.GetMask("Ground");
    }
    public void Refresh()
    {
        //should be done in one method just like setrunning;
        isGrounded = IsGrounded();
        SetJustJumped(false); // set it to false once it has detected if character just jumped or not
        character.animationsManager.SetGroundedVariable(isGrounded);
        SetRunning();
    }
    public void SetJustJumped(bool justJumped)
    {
        this.justJumped = justJumped;
    }
    public void SetRunning()
    {
        isRunning = isMoving && isGrounded;
        character.animationsManager.SetRunningVariable(isRunning);
    }
    public void SetMoving(bool isMoving)
    {
        this.isMoving = isMoving;
    }
    public void SetCrouching(bool isCrouching)
    {
        character.animationsManager.SetCrouching(isCrouching);
    }
    public bool IsActing()
    {
        return IsAttacking() || IsParrying();
    }
    public bool IsAttacking()
    {
        return character.animator.GetBool("attacking");
    }
    public bool IsParrying()
    {
        return character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "parry";
    }
    public bool IsCrouching()
    {
        return character.animator.GetBool("crouching");
    }
    public bool IsWatchingUpward()
    {
        return character.movementManager.isWatchingUpward;
    }

    public bool IsStunned()
    {
        return character.animator.GetBool("stunned");
    }
    public bool CanParry()
    {
        return !IsActing() && !IsStunned() && IsGrounded();
    }
    public bool CanCrouch()
    {
        return IsGrounded();
    }
    public bool CanAct()
    {
        return !IsActing() && !IsStunned();
    }
    bool IsGrounded()
    {
        //Raycast checks for collision, it has 3 parametters
        //vector3 origin, which is the starting point
        //vector3 or 2 direction, which is the direction in which it check a collision
        //float maxDistance, which is the maximum distance from which it needs to check a collision
        RaycastHit2D raycastHit = Physics2D.Raycast(character.boxCollider2D.bounds.center, Vector2.down, character.boxCollider2D.bounds.extents.y + 0.02f, ground);
        return raycastHit.collider != null && !justJumped;
    }
    public bool IsFalling()
    {
        return character.rb2d.velocity.y < -0.3;
    }
    public bool IsJumping()
    {
        return character.rb2d.velocity.y > 0.1; // should also detect if the character is grounded but stages dont move for now so :shrug:
    }
    public bool CanMove()
    {
        return !IsCrouching() && !IsStunned() && !IsActing() || !IsGrounded() && !IsStunned(); // can always move when airborn
    }
}
