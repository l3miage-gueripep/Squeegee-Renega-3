using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementManager : MonoBehaviour
{
    Character character;
    float currentMovementSpeed;
    public bool isWatchingUpward;
    int availableAirJumps = 1;
    int doneAirJumps;
    bool isBufferingJump;
    bool isBufferingFastFall;
    bool isFastFalling;
    bool isPressingDown;
    float shaking = 0.2f;
    private void Start()
    {
        character = GetComponent<Character>();
    }
    private void Update()
    {

    }
    public void Move()
    {
        // method is called move but you can move of a value of 0
        if (character.stateManager.CanMove())
        {
            if (character.stateManager.isGrounded)
            {
                MoveGrounded();
            }
            else
            {
                MoveAirborn();
            }
            character.stateManager.SetMoving(currentMovementSpeed != 0);
        }
        else
        {
            character.stateManager.SetMoving(false);
        }

    }
    public void SetLayer()
    {
        //prevents the players from colliding in air, is there a way to do that outside of update?
        if (character.stateManager.isGrounded)
        {
            ChangeLayer(character.characterLayer);
        }
        else
        {
            ChangeLayer(character.airbornCharacterLayer);
        }
    }
    public void MoveGrounded()
    {
        character.rb2d.velocity = new Vector2(currentMovementSpeed, character.rb2d.velocity.y); //move
        SetFacingSide(currentMovementSpeed);

    }
    public void MoveAirborn()
    {
        // if the direction pressed is the opposite of where the rigidbody is going to
        if(character.rb2d.velocity.x > 0 && currentMovementSpeed < 0 || character.rb2d.velocity.x < 0 && currentMovementSpeed > 0)
        {
            character.rb2d.AddForce(new Vector3(currentMovementSpeed * 5, 0));
        }
        else if(Mathf.Abs(character.rb2d.velocity.x) <= character.movementSpeed)
        {
            character.rb2d.AddForce(new Vector3(currentMovementSpeed * 5, 0));
        }
    }
    public void HandleDownInput(bool isPressingDown)
    {
        //crouch
        if (character.stateManager.isGrounded && !character.stateManager.justJumped && isPressingDown)
        {
            Stop();
            character.stateManager.SetCrouching(true);
        }
        else
        {
            character.stateManager.SetCrouching(false);
        }

        this.isPressingDown = isPressingDown;
        FastFallIfPossible(isPressingDown);


    }
    public IEnumerator FastFallWhenPossible()
    {
        while (isBufferingFastFall)
        {
            yield return new WaitForFixedUpdate();
            FastFallIfPossible(isPressingDown );
        }
    }
    public void FastFallIfPossible(bool isPressingDown)
    {
        if (isPressingDown) // wants to fast fall
        {
            if (!isFastFalling) // is not already fastfalling
            {
                if (character.stateManager.IsFalling()) // can fast fall
                {
                    StartCoroutine(FastFall());
                }
                else // otherwise
                {
                    if (!isBufferingFastFall) // if it's not already buffered
                    {
                        isBufferingFastFall = true;
                        StartCoroutine(FastFallWhenPossible());
                    }
                }
            }
        }
        else // doesn't want to fast fall, cancel the buffer if its buffered
        {
            if (isBufferingFastFall)
            {
                isBufferingFastFall = false;
            }
        }
    }
    public IEnumerator FastFall()
    {
        isFastFalling = true;
        CancelVerticalMomentum();
        character.rb2d.AddForce(new Vector3(0, -character.rb2d.mass*15), ForceMode2D.Impulse);
        while (!character.stateManager.isGrounded)
        {
            yield return new WaitForFixedUpdate();
        }
        isFastFalling = false;
        if (isPressingDown) // crouch if the down direction is still pressed
        {
            HandleDownInput(true);
        }
    }
    public void SetFacingSide(float moveValue)
    {
        // if facing side need to be changed, change it
        if (moveValue < 0 && Math.Round(transform.rotation.y) == 0 || moveValue > 0 && Math.Round(transform.rotation.y) != 0)
        {
            transform.Rotate(0, 180, 0);
        }
    }
    public int GetFacingSide()
    {
        if (Math.Round(character.transform.rotation.y) == 0)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    public void SetMovement(Vector2 inputValue)
    {

        Vector2 direction = new Vector2((float)Math.Round(inputValue.x), (float)Math.Round(inputValue.y)); //values are rounded to have fixed movement speed
        HandleDownInput(direction.y == -1); //crouch if the down direction is pressed, need the boolean to also disable crouch when it's not
        isWatchingUpward = (direction.y == 1);

        currentMovementSpeed = direction.x * character.movementSpeed;


    }
    public void Stop()
    {
        character.rb2d.velocity = new Vector2(0, character.rb2d.velocity.y);
    }
    public void JumpIfPossible(bool isPressed)
    {

        if (isPressed)
        {
            if (character.stateManager.CanAct())
            {
                if (character.stateManager.isGrounded)
                {
                    Jump();
                    ResetDoneAirJump();
                }
                else if (doneAirJumps < availableAirJumps)
                {
                    CancelVerticalMomentum();
                    Jump();
                    isFastFalling = false;
                    doneAirJumps++;
                }
                isBufferingJump = false;
            }
            else
            {
                if (!isBufferingJump)
                {
                    isBufferingJump = true;
                    StartCoroutine(JumpWhenPossible());
                }
            }
        }
        else
        {
            isBufferingJump = false;
        }
    }
    public void CancelVerticalMomentum()
    {
        character.rb2d.velocity = new Vector2(character.rb2d.velocity.x, 0); //cancels the current vertical momentum
    }
    public IEnumerator JumpWhenPossible()
    {
        while (isBufferingJump)
        {
            yield return new WaitForEndOfFrame();
            JumpIfPossible(isBufferingJump);
        }
    }
    public void Jump()
    {
        character.stateManager.SetJustJumped(true);
        character.stateManager.SetCrouching(false);
        character.rb2d.AddForce(new Vector3(0, character.jumpForce), ForceMode2D.Impulse); //jump
        character.animationsManager.Jump();
    }
    // change layer to prevent aerial collision 9 = ground 10 = air
    public void ChangeLayer(int layer)
    {
        gameObject.layer = layer;
    }
    public void ResetDoneAirJump()
    {
        doneAirJumps = 0;
    }
    public void TakeHit(Attack attack, Character hitter)
    {
        //insert delay for the hit effect
        ResetDoneAirJump();
        TakeKnockback(attack, hitter);
        SetFacingSide((float)-hitter.movementManager.GetFacingSide());
    }
    public void TakeKnockback(Attack attack, Character hitter)
    {
        float xKnockback = attack.knockBackDirection.x * 1 * hitter.movementManager.GetFacingSide() * attack.knockbackPower;
        float yKnockback = attack.knockBackDirection.y * 2 * attack.knockbackPower;
        character.rb2d.velocity = new Vector2(0, 0); //cancels current knockback
        character.rb2d.AddForce(new Vector3(xKnockback, yKnockback), ForceMode2D.Impulse);
    }

    public IEnumerator Shake()
    {
        for(int i = 0; i < GameManager.slowMotionDuration; i++)
        {
            shaking = -shaking;
            yield return new WaitForSeconds(1f / 60);
            transform.position = new Vector2(transform.position.x + shaking, transform.position.y);
        }
    }

}
